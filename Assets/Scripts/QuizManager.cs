using System.Collections;
using System.Collections.Generic;
using SpatialSys.UnitySDK;
using TMPro;
using UnityEngine;

namespace Simpleverse
{
    public class QuizManager : MonoBehaviour
    {

        [SerializeField]
        private QuizData quizData;
        [SerializeField]
        private GameObject questionObjectPrefab;
        [SerializeField]
        private GameObject optionObjectPrefab;
        [SerializeField]
        private Transform questionContainer;
        [SerializeField]
        private List<Transform> optionContainers;

        private int currentQuestionIndex = 0;
        private List<GameObject> instantiatedObjects = new List<GameObject>();
        private float score = 0.0f;
        private int pointsPerQuestion;
        private int totalPossibleScore;

        private List<string> wrongQuestions = new List<string>();

        void Start()
        {
            totalPossibleScore = quizData.questions.Count * pointsPerQuestion;
            pointsPerQuestion = 10;
        }

        public void DisplayQuestion()
        {
            // Get the current question
            Question question = quizData.questions[currentQuestionIndex];

            // Instantiate the question object and set the question text
            GameObject questionObject = Instantiate(questionObjectPrefab, questionContainer);
            questionObject.SetActive(true);
            instantiatedObjects.Add(questionObject);
            QuizQuestion questionObjectScript = questionObject.GetComponent<QuizQuestion>();
            questionObjectScript.SetQuestion(question.questionText);

            // Instantiate the option objects and set the option texts
            for (int i = 0; i < question.options.Count; i++)
            {
                GameObject optionObject = Instantiate(optionObjectPrefab, optionContainers[i]);
                optionObject.SetActive(true);
                instantiatedObjects.Add(optionObject);
                QuizOption optionObjectScript = optionObject.GetComponent<QuizOption>();
                optionObjectScript.SetOption(question.options[i].optionText, i == quizData.questions[currentQuestionIndex].correctAnswerIndex);
            }
        }

        public void OnOptionSelected(bool isCorrect)
        {
            if (isCorrect)
            {
                // add/calculate score 
                score += 10;
                score /= totalPossibleScore;
            }
            else
            {
                // If the wrong option was selected, save question
                string question = quizData.questions[currentQuestionIndex].questionText;
                wrongQuestions.Add(question);
                Debug.Log("ADDED WRONG QUESTION: " + question);
            }

            // ...go to the next question
            currentQuestionIndex++;
            if (currentQuestionIndex < quizData.questions.Count)
            {
                DisplayQuestion();
            }
            else
            {
                // If there are no more questions, end the quiz
                EndQuiz();
            }
        }

        void EndQuiz()
        {
            // Calculate score and show
            if (score == 0 || score < 60.0f)
            {
                Debug.Log("FAILED: " + score);
            }
            else
            {
                Debug.Log("YOU PASSED !" + score);

            }
        }

        public void Reset()
        {
            currentQuestionIndex = 0;
            score = 0;
            foreach (GameObject obj in instantiatedObjects)
            {
                Destroy(obj);
            }

            instantiatedObjects.Clear();
        }
    }


}

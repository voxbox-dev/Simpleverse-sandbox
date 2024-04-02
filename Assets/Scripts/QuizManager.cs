using System.Collections;
using System.Collections.Generic;
using SpatialSys.UnitySDK;
using TMPro;
using UnityEngine;

namespace Simpleverse
{
    public class QuizManager : MonoBehaviour
    {

        // Serialized Fields
        [SerializeField]
        private QuizData quizData;
        [SerializeField]
        private GameObject questionObjectPrefab;
        [SerializeField]
        private GameObject optionObjectPrefab;
        [SerializeField]
        private GameObject optionContainer;
        [SerializeField]
        private GameObject resultsPrefab;
        [SerializeField]
        private Transform resultsContainer;

        [SerializeField]
        private Transform questionContainer;
        [SerializeField]
        private List<Transform> optionContainers;
        [SerializeField]
        private int pointsPerQuestion;
        // Private Fields
        private int currentQuestionIndex = 0;
        private int score = 0;
        private int totalPossibleScore;
        private List<GameObject> instantiatedObjects = new();
        private List<string> wrongQuestions = new List<string>();
        private string currQuestionText;
        // Types
        private Question currQuestion;
        private QuizQuestion questionObjectScript;
        private QuizResults resultsObjectScript;
        // Objects
        private GameObject questionObject;
        private GameObject resultsObject;

        void Start()
        {
            totalPossibleScore = quizData.questions.Count * pointsPerQuestion;
            questionObjectScript = questionObjectPrefab.GetComponent<QuizQuestion>();
            resultsObjectScript = resultsPrefab.GetComponent<QuizResults>();
        }
        public void StartQuiz()
        {
            if (resultsPrefab) resultsPrefab.SetActive(false);

            currQuestion = quizData.questions[currentQuestionIndex];
            currQuestionText = currQuestion.questionText;

            if (currQuestion != null)
            {
                // Instantiate the question object and set the question text
                Debug.Log("QUestion text: " + currQuestionText);
                questionObjectScript.SetQuestion(currQuestionText);
                questionObject = Instantiate(questionObjectPrefab, questionContainer);
                instantiatedObjects.Add(questionObject);
                questionObject.SetActive(true);

                DisplayOptions(currQuestion);
            }
            else
            {
                // Instantiate the question object and set the question text
                Debug.Log("STARTQUIZ - QUESTION NULL:  " + currQuestion);
            }
        }
        public void DisplayOptions(Question question)
        {
            // Instantiate the option objects and set the option texts
            for (int i = 0; i < question.options.Count; i++)
            {
                // Must instantiate here, to set the option properties per object correctly
                GameObject optionObject = Instantiate(optionObjectPrefab, optionContainers[i]);
                optionObject.SetActive(true);
                QuizOption optionObjectScript = optionObject.GetComponent<QuizOption>();
                optionObjectScript.SetOption(question.options[i].optionText, i == question.correctAnswerIndex);
                instantiatedObjects.Add(optionObject);

                // Debug.Log("OPTION index: " + i + " isCorrect Answer ? " + question.correctAnswerIndex);
            }
        }
        public void OnOptionSelected(bool isCorrect)
        {
            if (isCorrect)
            {
                // add/calculate score 
                score += 10;
                score /= totalPossibleScore;
                Debug.Log("CORRECT" + score);
            }
            else
            {
                // If the wrong option was selected, save question
                string question = currQuestion.questionText;
                wrongQuestions.Add(question);
                Debug.Log("ADDED WRONG QUESTION: " + question);
            }

            // If there are more questions, go to the next question
            if (currentQuestionIndex + 1 < quizData.questions.Count)
            {
                currentQuestionIndex++;
                StartQuiz();
            }
            else
            {
                // If there are no more questions, end the quiz
                EndQuiz();
            }
        }
        void EndQuiz()
        {
            DestroyInstantiatedObjs();
            DisplayResults();
        }
        public void DisplayResults()
        {
            string result;
            int finalScore = score * 100;
            string scoreText = finalScore + "%";

            // Pass or Fail?
            if (score < 1)
            {
                result = "FAIL";
                resultsObjectScript.ShowResultsAction(false);
            }
            else
            {
                result = "PASS";
                resultsObjectScript.ShowResultsAction(true);
            }
            // Set results text
            resultsObjectScript.SetResultsText(result);
            resultsObjectScript.SetResultsScoreText(scoreText);

            // Show results panel
            if (!instantiatedObjects.Contains(resultsPrefab))
            {
                resultsObject = Instantiate(resultsPrefab, resultsContainer);
                instantiatedObjects.Add(resultsObject);
            }
            resultsObject.SetActive(true);
        }

        void HideQuizObjects()
        {
            // Hide the question and option objects
            foreach (GameObject obj in instantiatedObjects)
            {
                obj.SetActive(false);
            }

        }
        public void RestartQuiz()
        {
            // If hiding objects, Show the question and option objects
            // foreach (GameObject obj in instantiatedObjects)
            // {
            //     obj.SetActive(true);
            // }

            // Hide the results object
            resultsObject.SetActive(false);

            StartQuiz();

        }
        public void ClaimPrize()
        {
            DestroyInstantiatedObjs();
            Reset();
        }
        public void DestroyInstantiatedObjs()
        {
            foreach (GameObject obj in instantiatedObjects)
            {
                Destroy(obj);
            }
            instantiatedObjects.Clear();
        }
        public void Reset()
        {
            currentQuestionIndex = 0;
            score = 0;
        }
    }


}

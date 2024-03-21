using System.Collections;
using System.Collections.Generic;
using SpatialSys.UnitySDK;
using TMPro;
using UnityEngine;

namespace Simpleverse
{
    public class QuizOption : MonoBehaviour
    {
        public TMP_Text optionText;
        private bool isCorrect;
        private QuizManager quizManager;
        void Start()
        {
            quizManager = FindObjectOfType<QuizManager>();
        }

        public void SetOption(string text, bool isCorrect)
        {
            optionText.text = text;
            this.isCorrect = isCorrect;
        }

        public void OnOptionSelected()
        {
            quizManager.OnOptionSelected(isCorrect);
        }

    }
}

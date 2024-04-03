using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace Simpleverse
{
    public class QuizResults : MonoBehaviour
    {

        public TMP_Text resultsText;
        public TMP_Text resultsScoreText;

        public void SetResultsText(string text)
        {
            resultsText.text = text;
        }
        public void SetResultsScoreText(string text)
        {
            resultsScoreText.text = text;
        }

    }
}

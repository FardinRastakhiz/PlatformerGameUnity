using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ThePotentialJump.Menus
{
    public class ShowResult : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI answerText;
        
        public void ShowCorrectAnswer(string correctAnswer)
        {
            answerText.text = correctAnswer;
        }
    }
}
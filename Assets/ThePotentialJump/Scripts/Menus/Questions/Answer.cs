using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ThePotentialJump.Menus
{
    public class Answer : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI answerId;
        [SerializeField] private TextMeshProUGUI answerText;
        public event EventHandler AnswerSelected ;

        public string AnswerId => answerId.text;
        public void AddAnswer(string answerId, string answerText)
        {
            this.answerId.text = answerId;
            this.answerText.text = answerText;
            button.onClick.AddListener(() => AnswerSelected?.Invoke(this, null));
        }

        internal void SetDisabledColor(Color color)
        {
            var colors = button.colors;
            colors.disabledColor = color;
            button.colors = colors;
        }
        internal void IntensifyColor()
        {
            var colors = button.colors;
            colors.disabledColor = new Color(colors.disabledColor.r, colors.disabledColor.g, colors.disabledColor.b, 1.0f);
            button.colors = colors;
        }
    }
}

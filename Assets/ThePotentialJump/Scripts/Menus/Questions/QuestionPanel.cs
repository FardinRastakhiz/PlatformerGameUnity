using LoLSDK;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ThePotentialJump.Menus
{

    public class QuestionPanel : MonoBehaviour
    {
        [SerializeField] private Answer answerPrefab;
        [SerializeField] private RectTransform answerParent;
        [SerializeField] private Button submitButton;
        [SerializeField] private int questionIndex;

        [Space]
        [SerializeField] private UnityEvent AnsweredCorrectly;
        [SerializeField] private UnityEvent AnsweredIncorrectly;


        private CanvasGroup canvasGroup;
        private CanvasGroupFadInOut canvasGroupFadInOut;
        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroupFadInOut = GetComponent<CanvasGroupFadInOut>();
        }

        private List<Answer> answers = new List<Answer>();

        private MultipleChoiceQuestion question;
        private Answer selectedAnswer;
        public void OnAskQuestion()
        {
            canvasGroupFadInOut.FadeIn();
            question = SharedState.QuestionList.questions[questionIndex];
            submitButton.interactable = false;
            submitButton.onClick.RemoveAllListeners();
            submitButton.onClick.AddListener(SubmitAnswer);


            for (int i = 0; i < question.alternatives.Length; i++)
            {
                int idx = System.Array.IndexOf(question.alternatives, question.alternatives[i]) + 1;

                Answer answer = Instantiate(answerPrefab, answerParent);
                answer.AddAnswer(question.alternatives[i].alternativeId, question.alternatives[i].text);

                answer.AnswerSelected += OnSelectAnswer;
                answers.Add(answer);
            }
        }



        public void OnSelectAnswer(object o, EventArgs e)
        {
            if (o is Answer answer)
            {
                selectedAnswer = answer;
                submitButton.interactable = true;
            }
        }

        public void SubmitAnswer()
        {
            for (int i = 0; i < answers.Count; i++)
            {

                if (question.correctAlternativeId == answers[i].AnswerId)
                {
                    answers[i].SetDisabledColor(new Color(0.0f, 1.0f, 0.5f, 0.25f));
                }
                else
                {
                    answers[i].SetDisabledColor(new Color(1.0f, 0.25f, 0.0f, 0.25f));
                }
            }
            selectedAnswer.IntensifyColor();
            if (question.correctAlternativeId == selectedAnswer.AnswerId)
                AnsweredCorrectly?.Invoke();
            else
                AnsweredIncorrectly?.Invoke();
            canvasGroup.interactable = false;
        }

    }
}

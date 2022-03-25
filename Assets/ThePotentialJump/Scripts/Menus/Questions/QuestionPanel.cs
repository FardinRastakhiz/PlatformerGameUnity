using LoLSDK;
using System;
using System.Collections.Generic;
using System.Text;
using ThePotentialJump.EditorUtilities;
using ThePotentialJump.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ThePotentialJump.Menus
{

    public class QuestionPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI questionTitle;
        [SerializeField] private TextMeshProUGUI questionStem;
        [SerializeField] private Answer answerPrefab;
        [SerializeField] private RectTransform answerParent;
        [SerializeField] private Button submitButton;
        [SerializeField] private int questionIndex;

        [Space]
        [SerializeField] private UnityEvent AnsweredCorrectly;
        [SerializeField] private UnityEvent AnsweredIncorrectly;
        private SpeechHandler speechHandler;


        private CanvasGroup canvasGroup;
        private CanvasGroupFadInOut canvasGroupFadInOut;
        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroupFadInOut = GetComponent<CanvasGroupFadInOut>();
            speechHandler = GetComponent<SpeechHandler>();
        }

        private List<Answer> answers = new List<Answer>();

        private MultipleChoiceQuestion question;
        private Answer selectedAnswer;
        public void OnAskQuestion()
        {
            if (canvasGroupFadInOut == null)
            {
                Debug.LogError("(QuestionPanel) canvasGroupFadInOut cannot be null!");
                return;
            }
            canvasGroupFadInOut.FadeIn();
            submitButton.interactable = false;
            submitButton.onClick.RemoveAllListeners();
            submitButton.onClick.AddListener(SubmitAnswer);

            if (SharedState.QuestionList == null || SharedState.QuestionList.questions == null || SharedState.QuestionList.questions.Length == 0)
            {
                submitButton.interactable = true;
                return;
            }
            // string questionKey = $"q{questionIndex}";
            
            question = SharedState.QuestionList.questions[questionIndex];
            if (question == null || question.alternatives == null || question.alternatives.Length == 0)
            {
                submitButton.interactable = true;
                return;
            }

            StringBuilder textForSpeech = new StringBuilder(question.stem);
            textForSpeech.Append("  \n");

            questionTitle.text = $"Question {questionIndex + 1}";
            questionStem.text = question.stem;
            for (int i = 0; i < question.alternatives.Length; i++)
            {
                int idx = System.Array.IndexOf(question.alternatives, question.alternatives[i]) + 1;

                Answer answer = Instantiate(answerPrefab, answerParent);
                answer.AddAnswer(question.alternatives[i].alternativeId, question.alternatives[i].text, i);
                textForSpeech.Append(i + 1).Append(",  ").Append(question.alternatives[i].text).Append(",  \n");
                answer.AnswerSelected += OnSelectAnswer;
                answers.Add(answer);
            }
            speechHandler.OnClickSpeakText(textForSpeech.ToString());
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
            if (SharedState.QuestionList == null || SharedState.QuestionList.questions == null || SharedState.QuestionList.questions.Length == 0)
            {
                AnsweredCorrectly?.Invoke();
                EconomySystem.Instance?.Deposit(8);
                canvasGroup.interactable = false;
                return;
            }
            if (question == null || question.alternatives == null || question.alternatives.Length == 0)
            {
                AnsweredCorrectly?.Invoke();
                EconomySystem.Instance?.Deposit(8);
                canvasGroup.interactable = false;
                return;
            }
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
            {
                EconomySystem.Instance?.Deposit(8);
                AnsweredCorrectly?.Invoke();
            }
            else
            {
                EconomySystem.Instance?.Withdraw(2);
                AnsweredIncorrectly?.Invoke();
            }
            canvasGroup.interactable = false;
        }

    }
}

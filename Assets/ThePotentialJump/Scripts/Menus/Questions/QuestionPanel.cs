using LoLSDK;
using System;
using System.Collections;
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
        [SerializeField] private ShowResult showCorrectResult;

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

            string questionKey = $"q{questionIndex}";
            speechHandler.CancelText();
            speechHandler.OnClickSpeakText($"{questionKey}tts");


            if (SharedState.LanguageDefs?[questionKey] == null)
            {
                submitButton.interactable = true;
                return;
            }
            var questionText = SharedState.LanguageDefs?[questionKey];


            questionTitle.text = $"Question {questionIndex}";
            questionStem.text = questionText;
            for (int i = 0; i < 4; i++)
            {
                Answer answer = Instantiate(answerPrefab, answerParent);
                answer.AddAnswer($"{questionKey}a{(i + 1).ToString()}",
                    SharedState.LanguageDefs?[$"{questionKey}a{(i + 1).ToString()}"], i);
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
            string questionKey = $"q{questionIndex}";
            if (SharedState.LanguageDefs?[questionKey] == null)
            {
                AnsweredCorrectly?.Invoke();
                EconomySystem.Instance?.Deposit(8);
                canvasGroup.interactable = false;
                return;
            }
            var correctId = SharedState.LanguageDefs?[$"{questionKey}at"].Value;
            var correctKey = $"{questionKey}a{correctId}";
            for (int i = 0; i < answers.Count; i++)
            {

                if (correctKey == answers[i].AnswerId)
                {
                    answers[i].SetDisabledColor(new Color(0.0f, 1.0f, 0.5f, 0.25f));
                }
                else
                {
                    answers[i].SetDisabledColor(new Color(1.0f, 0.25f, 0.0f, 0.25f));
                }
            }
            selectedAnswer.IntensifyColor();
            // canvasGroup.interactable = false;
            if (correctKey == selectedAnswer.AnswerId)
            {
                EconomySystem.Instance?.Deposit(8);
                AnsweredCorrectly?.Invoke();
            }
            else
            {
                string correctAnswer = SharedState.LanguageDefs?[$"{questionKey}dcr"].Value;
                speechHandler.CancelText();
                speechHandler.OnClickSpeakText($"{questionKey}dcr");
                showCorrectResult.ShowCorrectAnswer(correctAnswer);
                EconomySystem.Instance?.Withdraw(2);
                AnsweredIncorrectly?.Invoke();
            }
        }

    }
}

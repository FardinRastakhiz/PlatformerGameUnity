using UnityEngine;
using Newtonsoft.Json;
using TMPro;
using System.Collections;
using System.Text;
using UnityEngine.UI;
using System;

namespace ThePotentialJump.Dialogues
{
    public class DialogueSystem : Utilities.MonoSingleton<DialogueSystem>
    {
        [Space]
        [Header("Files")]
        [SerializeField] private TextAsset dialoguesJSON;
        [Space]
        [Header("UIs")]
        [SerializeField] private CanvasGroup dialogueUIGroup;
        [SerializeField] private TextMeshProUGUI dialogueTitle;
        [SerializeField] private TextMeshProUGUI dialogueTextField;
        [SerializeField] private Button closeButton;
        [Space]
        [Header("Typing characters")]
        [SerializeField] private int characterPerSecond = 5;
        [Space]
        [Header("Fade in fade out")]
        [SerializeField] private float fadeInDuration = 0.5f;
        [SerializeField] private float fadeOutDuration = 0.5f;


        private DialogueSequence dialogues;
        Coroutine fillDialogueCoroutine;
        Coroutine fadeOutCoroutine;
        protected override void Awake()
        {
            destroyOnLoad = true;
            base.Awake();
            dialogues = JsonConvert.DeserializeObject<DialogueSequence>(dialoguesJSON.text);
            closeButton.onClick.AddListener(() => OnCloseButtonClicked());
        }

        private void Start()
        {
            //StartCoroutine(TestDialogue());
        }

        IEnumerator TestDialogue()
        {
            yield return new WaitForSeconds(3);
            OnPlayDialogueSection("stage2", "dialogue01");
        }

        public void OnCloseButtonClicked()
        {
            closeButton.interactable = false;
            if (fillDialogueCoroutine != null) StopCoroutine(fillDialogueCoroutine);
            if (fadeOutCoroutine != null) StopCoroutine(fadeOutCoroutine);
            fadeOutCoroutine = StartCoroutine(FadeOut());
            onFinishSectionAction?.Invoke();
        }

        private Action onFinishSectionAction = null;

        public void OnPlayDialogueSection(string stage,
                                            string section,
                                            Action onFinishSectionAction = null,
                                            float beginPause = 0.0f,
                                            float endingPause = 0.0f)
        {
            this.onFinishSectionAction = onFinishSectionAction;
            var ds = dialogues[stage][section];
            dialogueTitle.text = ds.Speaker;
            dialogueTitle.alignment = ds.TitleAlignment;
            dialogueTextField.text = "";
            closeButton.interactable = false;

            if (fillDialogueCoroutine != null) StopCoroutine(fillDialogueCoroutine);
            if (fadeOutCoroutine != null) StopCoroutine(fadeOutCoroutine);
            fillDialogueCoroutine = StartCoroutine(FillDialogue(ds.Paragraph, beginPause, endingPause));
        }

        private IEnumerator FillDialogue(string dialogueText, float beginPause = 0.0f, float endingPause = 0.0f)
        {
            yield return FadeIn();
            yield return new WaitForSeconds(beginPause);
            StringBuilder sb = new StringBuilder();
            var waitTime = new WaitForSeconds(1.0f / (characterPerSecond * 1.0f));
            for(int i = 0; i< dialogueText.Length; i++)
            {
                sb.Append(dialogueText[i]);
                dialogueTextField.text = sb.ToString();
                yield return waitTime;
            }
            closeButton.interactable = true;
            yield return new WaitForSeconds(endingPause);
            //yield return new WaitForSeconds(0.2f);
            OnCloseButtonClicked();
        }

        IEnumerator FadeIn()
        {
            while (dialogueUIGroup.alpha < 1)
            {
                dialogueUIGroup.alpha += Time.deltaTime * 1.0f / fadeInDuration;
                yield return null;
            }
            dialogueUIGroup.interactable = true;
            dialogueUIGroup.blocksRaycasts = true;
        }
        IEnumerator FadeOut()
        {
            while (dialogueUIGroup.alpha > 0)
            {
                dialogueUIGroup.alpha -= Time.deltaTime * 1.0f / fadeOutDuration;
                yield return null;
            }
            dialogueUIGroup.interactable = false;
            dialogueUIGroup.blocksRaycasts = false;
        }
    }

}

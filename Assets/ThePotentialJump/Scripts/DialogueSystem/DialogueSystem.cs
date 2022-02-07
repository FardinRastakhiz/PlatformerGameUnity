using UnityEngine;
using Newtonsoft.Json;
using TMPro;
using System.Collections;
using System.Text;
using UnityEngine.UI;

namespace ThePotentialJump.Dialogues
{
    public class DialogueSystem : Utilities.Singleton<DialogueSystem>
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

        private void OnCloseButtonClicked()
        {
            closeButton.interactable = false;
            if (fillDialogueCoroutine != null) StopCoroutine(fillDialogueCoroutine);
            if (fadeOutCoroutine != null) StopCoroutine(fadeOutCoroutine);
            fadeOutCoroutine = StartCoroutine(FadeOut());
        }

        public void OnPlayDialogueSection(string stage, string section)
        {

            var ds = dialogues[stage][section];
            dialogueTitle.text = ds.Speaker;
            dialogueTitle.alignment = ds.TitleAlignment;
            dialogueTextField.text = "";
            closeButton.interactable = false;

            if (fillDialogueCoroutine != null) StopCoroutine(fillDialogueCoroutine);
            if (fadeOutCoroutine != null) StopCoroutine(fadeOutCoroutine);
            fillDialogueCoroutine = StartCoroutine(FillDialogue(ds.Paragraph));
        }

        private IEnumerator FillDialogue(string dialogueText)
        {
            yield return FadeIn();

            StringBuilder sb = new StringBuilder();
            var waitTime = new WaitForSeconds(1.0f / (characterPerSecond * 1.0f));
            for(int i = 0; i< dialogueText.Length; i++)
            {
                sb.Append(dialogueText[i]);
                dialogueTextField.text = sb.ToString();
                yield return waitTime;
            }
            closeButton.interactable = true;
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

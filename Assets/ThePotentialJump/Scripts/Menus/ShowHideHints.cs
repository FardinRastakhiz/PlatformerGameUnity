using System.Collections;
using System.Collections.Generic;
using ThePotentialJump.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ThePotentialJump.Menus
{
    public class ShowHideHints : MonoBehaviour
    {

        [SerializeField] private bool playOnAwake = false;
        [SerializeField] private Button showHideButton;
        [SerializeField] private Sprite showButtonIcon;
        [SerializeField] private Sprite hideButtonIcon;
        [SerializeField] private Image showHideButtonIconImage;
        [SerializeField] private TextMeshProUGUI showHideButtonText;

        private FadeInOutCanvasGroup[] hintFadeInOuts;
        private void Awake()
        {
            hintFadeInOuts = GetComponentsInChildren<FadeInOutCanvasGroup>();
            showHideButton.onClick.AddListener(Show);
        }
        private void Start()
        {
            canShowCoroutine = StartCoroutine(WaitForShowHintButton());
        }

        public void Show()
        {
            for (int i = 0; i < hintFadeInOuts.Length; i++)
            {
                hintFadeInOuts[i].FadeIn();
            }

            showHideButton.onClick.RemoveAllListeners();
            showHideButton.onClick.AddListener(Hide);
            showHideButtonIconImage.sprite = hideButtonIcon;
            showHideButtonText.text = " Hide Hints";
            if (canShowCoroutine != null) StopCoroutine(canShowCoroutine);
            canShowCoroutine = StartCoroutine(WaitForShowHintButton());
        }



        public void Hide()
        {
            if (canShowCoroutine != null) StopCoroutine(canShowCoroutine);
            for (int i = 0; i < hintFadeInOuts.Length; i++)
            {
                hintFadeInOuts[i].FadeOut();
            }
            showHideButton.onClick.RemoveAllListeners();
            showHideButton.onClick.AddListener(Show);
            showHideButtonIconImage.sprite = showButtonIcon;
            showHideButtonText.text = " Show Hints";
        }

        [Space]
        [SerializeField] private float waitForShowAgain = 7;
        Coroutine canShowCoroutine;
        IEnumerator WaitForShowHintButton()
        {
            yield return new WaitForSeconds(waitForShowAgain);
            showHideButton.onClick.RemoveAllListeners();
            showHideButton.onClick.AddListener(Show);
            showHideButtonIconImage.sprite = showButtonIcon;
            showHideButtonText.text = " Show Hints";
        }
    }
}

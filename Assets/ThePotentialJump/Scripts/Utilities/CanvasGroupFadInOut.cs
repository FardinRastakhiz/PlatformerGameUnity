using System.Collections;
using UnityEngine;

namespace ThePotentialJump.EditorUtilities
{
    public class CanvasGroupFadInOut : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;

        private void Awake()
        {
            if (!canvasGroup) canvasGroup = GetComponent<CanvasGroup>();
        }
        private Coroutine fadeInCoroutine;
        private Coroutine fadeOutCoroutine;
        public void FadeIn()
        {
            if (fadeOutCoroutine != null) StopCoroutine(fadeOutCoroutine);
            fadeInCoroutine = StartCoroutine(StartFadingIn());
        }

        public void FadeOut()
        {
            if (fadeInCoroutine != null) StopCoroutine(fadeInCoroutine);
            fadeOutCoroutine = StartCoroutine(StartFadingOut());
        }

        IEnumerator StartFadingIn()
        {
            while (canvasGroup.alpha < 1 - float.Epsilon)
            {
                canvasGroup.alpha += Time.deltaTime * 2;
                yield return null;
            }
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        IEnumerator StartFadingOut()
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            while (canvasGroup.alpha > float.Epsilon)
            {
                canvasGroup.alpha -= Time.deltaTime * 2;
                yield return null;
            }
        }

    }
}
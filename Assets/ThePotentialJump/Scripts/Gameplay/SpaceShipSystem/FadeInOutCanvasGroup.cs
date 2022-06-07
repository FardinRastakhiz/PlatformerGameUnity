using System.Collections;
using UnityEngine;

namespace ThePotentialJump.Gameplay
{
    public class FadeInOutCanvasGroup : MonoBehaviour
    {
        [SerializeField] private float fadeInDuration = 1.0f;
        [SerializeField] private float fadeOutDuration = 1.0f;
        [SerializeField] private float presentDuration = 5.0f;
        [Space]
        [SerializeField] private bool blockRaycast = true;
        [SerializeField] private bool interactable = true;

        [Space]
        [SerializeField] private CanvasGroup canvasGroup;

        [Space]
        [SerializeField] private bool scheduled = true;

        private Coroutine scheduleCoroutine;
        private Coroutine fadeInCoroutine;
        private Coroutine fadeOutCoroutine;
        private void OnEnable()
        {
            if (!scheduled) return;
            if (scheduleCoroutine != null) StopCoroutine(scheduleCoroutine);
            scheduleCoroutine = StartCoroutine(Scheduling());
        }

        private void OnDisable()
        {
            StopCoroutines();
        }


        public void FadeIn()
        {
            StopCoroutines();

            if (scheduled)
                scheduleCoroutine = StartCoroutine(Scheduling());
            else
                fadeInCoroutine = StartCoroutine(FadeInCoroutine(fadeInDuration));
        }
        public void FadeOut()
        {
            StopCoroutines();
            fadeOutCoroutine = StartCoroutine(FadeOutCoroutine(fadeOutDuration));
        }


        IEnumerator Scheduling()
        {
            yield return FadeInCoroutine(fadeInDuration);
            yield return new WaitForSeconds(presentDuration);
            yield return FadeOutCoroutine(fadeOutDuration);
        }


        IEnumerator FadeInCoroutine(float fadeInDuration)
        {
            canvasGroup.interactable = blockRaycast;
            canvasGroup.blocksRaycasts = interactable;
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime * 1.0f / fadeInDuration;
                yield return null;
            }
        }
        IEnumerator FadeOutCoroutine(float fadeOutDuration)
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime * 1.0f / fadeOutDuration;
                yield return null;
            }
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }


        private void StopCoroutines()
        {
            if (scheduleCoroutine != null) StopCoroutine(scheduleCoroutine);
            if (fadeInCoroutine != null) StopCoroutine(fadeInCoroutine);
            if (fadeOutCoroutine != null) StopCoroutine(fadeOutCoroutine);
        }
    }
}
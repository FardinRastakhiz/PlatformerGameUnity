using System.Collections;
using UnityEngine;

public class FadeInOutCanvasGroup : MonoBehaviour
{
    [SerializeField] private float fadeInDuration = 1.0f;
    [SerializeField] private float fadeOutDuration = 1.0f;
    [SerializeField] private float presentDuration = 5.0f;

    [Space]
    [SerializeField] private CanvasGroup canvasGroup;

    private Coroutine scheduleCoroutine;
    private void OnEnable()
    {
        if (scheduleCoroutine != null) StopCoroutine(scheduleCoroutine);
        scheduleCoroutine = StartCoroutine(scheduling());
    }

    private void OnDisable()
    {
        if (scheduleCoroutine != null) StopCoroutine(scheduleCoroutine);
    }

    IEnumerator scheduling()
    {
        yield return FadeIn(fadeInDuration);
        yield return new WaitForSeconds(presentDuration);
        yield return FadeOut(fadeOutDuration);
    }

    IEnumerator FadeIn(float fadeInDuration)
    {
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime * 1.0f / fadeInDuration;
            yield return null;
        }
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    IEnumerator FadeOut(float fadeOutDuration)
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * 1.0f / fadeOutDuration;
            yield return null;
        }
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

}

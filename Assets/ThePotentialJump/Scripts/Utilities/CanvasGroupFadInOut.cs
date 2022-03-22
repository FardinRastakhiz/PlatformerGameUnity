using System.Collections;
using UnityEngine;

public class CanvasGroupFadInOut : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    private void Awake()
    {
        if (canvasGroup) canvasGroup = GetComponent<CanvasGroup>();
    }

    public void FadeIn()
    {
        StartCoroutine(StartFadingIn());
    }

    public void FadeOut()
    {
        StartCoroutine(StartFadingOut());
    }

    IEnumerator StartFadingIn()
    {
        while (canvasGroup.alpha<1-float.Epsilon)
        {
            canvasGroup.alpha += Time.deltaTime*2;
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

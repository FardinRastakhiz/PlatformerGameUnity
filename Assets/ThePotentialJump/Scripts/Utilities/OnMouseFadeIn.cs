using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ThePotentialJump.Utilities
{


    public class OnMouseFadeIn : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float fadingRate = 1.0f;
        [SerializeField] private bool reverse = false;

        private CancellationToken cancellation = new CancellationToken();

        private void Awake()
        {
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (reverse)
            {
                cancellation.IsCancelled = false;
                StartCoroutine(FadeIn());
            }
            else
            {
                cancellation.IsCancelled = true;
                StartCoroutine(FadeOut());
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (reverse)
            {
                cancellation.IsCancelled = true;
                StartCoroutine(FadeOut());
            }
            else
            {
                cancellation.IsCancelled = false;
                StartCoroutine(FadeIn());
            }
        }

        IEnumerator FadeIn()
        {
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime * fadingRate;
                yield return null;
                if(cancellation.IsCancelled) yield break;
            }
        }

        IEnumerator FadeOut()
        {
            while (canvasGroup.alpha > 0.05f)
            {
                canvasGroup.alpha -= Time.deltaTime * fadingRate;
                yield return null;
                if (!cancellation.IsCancelled) yield break;
            }
        }

    }
}
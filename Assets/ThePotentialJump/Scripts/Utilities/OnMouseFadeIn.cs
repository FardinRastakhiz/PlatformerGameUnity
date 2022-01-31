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

        private CancellationToken cancellation = new CancellationToken();

        private void Awake()
        {
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            cancellation.IsCancelled = true;
            StartCoroutine(FadeOut());
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            cancellation.IsCancelled = false;
            StartCoroutine(FadeIn());
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
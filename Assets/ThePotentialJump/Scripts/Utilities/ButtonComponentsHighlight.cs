using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ThePotentialJump.Utilities
{

    public class ButtonComponentsHighlight : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler
    {

        [SerializeField] private Graphic[] targetElements;

        [SerializeField] private float fadingRate = 1.0f;
        [SerializeField] private Color targetColor = Color.green;

        private CancellationToken cancellation = new CancellationToken();
        private Color updateColor;

        private void Awake()
        {
            if (targetElements.Length == 0)
            {
                Debug.LogError("Highlightable components are not assigned");
                Destroy(this);
                return;
            }

            updateColor = targetElements[0].color;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            cancellation.IsCancelled = false;
            StartCoroutine(FadeIn());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            cancellation.IsCancelled = true;
            StartCoroutine(FadeOut());
        }

        IEnumerator FadeIn()
        {
            var color_direction = targetColor - updateColor;
            while (color_direction.Norm2() > 0.1)
            {
                updateColor = updateColor.AddColor(color_direction * Time.deltaTime * fadingRate);
                UpdateButtonColor(updateColor);
                yield return null;
                if (cancellation.IsCancelled) yield break;
            }
        }

        IEnumerator FadeOut()
        {
            var color_direction = Color.white - updateColor;
            while (color_direction.Norm2() > 0.1)
            {
                updateColor = updateColor.AddColor(color_direction * Time.deltaTime * fadingRate);
                UpdateButtonColor(updateColor);
                yield return null;
                if (!cancellation.IsCancelled) yield break;
            }
        }


        private void UpdateButtonColor(Color color)
        {
            for (int i = 0; i < targetElements.Length; i++)
                targetElements[i].color = color;
        }
    }
}
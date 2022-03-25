using System.Collections;
using UnityEngine;

namespace ThePotentialJump.Utilities
{
    public class TemporaryButton : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;

        public void Activate()
        {
            StartCoroutine(ActivateButton());
        }

        public void Deactivate()
        {
            canvasGroup.interactable = false;
            StartCoroutine(DeactivateButton());
        }

        IEnumerator ActivateButton()
        {
            while (canvasGroup.alpha < 1 - float.Epsilon)
            {
                canvasGroup.alpha += Time.deltaTime * 3;
                yield return null;
            }
            canvasGroup.interactable = true;
        }

        IEnumerator DeactivateButton()
        {
            while (canvasGroup.alpha > float.Epsilon*4)
            {
                canvasGroup.alpha -= Time.deltaTime * 3;
                yield return null;
            }
        }
    }

}
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
            while (canvasGroup.alpha > float.Epsilon)
            {
                canvasGroup.alpha -= Time.deltaTime * 3;
                yield return null;
            }
            canvasGroup.interactable = false;
        }
    }

}
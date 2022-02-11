using System.Collections;
using UnityEngine;

namespace ThePotentialJump.Gameplay
{
    public class JumpRuler : MonoBehaviour
    {
        [SerializeField] private RectTransform ruler;
        private CanvasGroup canvasGroup;

        private WaitForSeconds waitForSeconds;
        private void Awake()
        {
            waitForSeconds = new WaitForSeconds(Time.fixedDeltaTime);
            canvasGroup = GetComponent<CanvasGroup>();
        }

        private Coroutine updateRulerCoroutine;
        public void OnStartUpdateRuler(Transform targetBinding)
        {
            canvasGroup.alpha = 1.0f;
            ruler.position = targetBinding.position + offset;
            updateRulerCoroutine = StartCoroutine(UpdateRuler(targetBinding));
        }

        public void OnStopUpdateRuler()
        {
            if (updateRulerCoroutine != null)
                StopCoroutine(updateRulerCoroutine);

        }

        public void OnHideRuler()
        {
            if (canvasGroup.alpha < 0.5f) return;
            if (updateRulerCoroutine != null)
                StopCoroutine(updateRulerCoroutine);
            canvasGroup.alpha = 0.0f;
        }

        [Space]
        [Header("Ruler view settings")]
        [SerializeField] private float scale;
        [SerializeField] private float baseSize;
        [SerializeField] private Vector3 offset;
        IEnumerator UpdateRuler(Transform targetBinding)
        {
            while (true)
            {
                var height = (targetBinding.position.y - ruler.position.y + baseSize) * scale;
                ruler.sizeDelta = new Vector2(ruler.sizeDelta.x, height);
                yield return waitForSeconds;
            }
        }
    }
}
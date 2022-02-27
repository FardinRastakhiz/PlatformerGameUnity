using System.Collections;
using UnityEngine;

namespace ThePotentialJump.Gameplay
{
    public class JumpRuler : MonoBehaviour
    {
        [SerializeField] private RectTransform ruler;
        [SerializeField] private bool reverseTracking;
        [SerializeField] private float maxHeight;
        private CanvasGroup canvasGroup;

        private WaitForSeconds waitForSeconds;
        private void Awake()
        {
            waitForSeconds = new WaitForSeconds(Time.fixedDeltaTime);
            canvasGroup = GetComponent<CanvasGroup>();
        }

        private Coroutine updateRulerCoroutine;
        private Vector3 startPosition = Vector3.zero;
        public void OnStartUpdateRuler(Transform targetBinding)
        {
            ResetRuler();
            canvasGroup.alpha = 1.0f;
            if (reverseTracking)
            {
                startPosition = targetBinding.position + offset;
                ruler.position = startPosition;
                updateRulerCoroutine = StartCoroutine(UpdateReverseRuler(targetBinding));
            }
            else
            {
                ruler.position = targetBinding.position + offset;
                updateRulerCoroutine = StartCoroutine(UpdateRuler(targetBinding));
            }
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
                //if(height<0)
                ruler.sizeDelta = new Vector2(ruler.sizeDelta.x, height);
                yield return waitForSeconds;
            }
        }
        IEnumerator UpdateReverseRuler(Transform targetBinding)
        {
            while (true)
            {
                var height = (startPosition.y - targetBinding.position.y + baseSize) * scale;
                if (height > maxHeight * scale || height < 0)
                {
                    ResetRuler();
                    yield break;
                }
                //if(height<0)
                ruler.sizeDelta = new Vector2(ruler.sizeDelta.x, height);
                var currentPosition = ruler.position;
                currentPosition.y = startPosition.y - height * ruler.lossyScale.y;
                ruler.position = currentPosition;
                yield return waitForSeconds;
            }
        }

        private void ResetRuler()
        {
            ruler.sizeDelta = new Vector2(ruler.sizeDelta.x, 0.0f);
        }
    }
}
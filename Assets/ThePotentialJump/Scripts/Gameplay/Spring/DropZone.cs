using System.Collections;
using TMPro;
using UnityEngine;

namespace ThePotentialJump.Gameplay
{
    public class DropZone : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Collider2D platformCollider;
        [SerializeField] private TextMeshProUGUI[] heightLabels;

        private Transform baseTransform;


        private void OnEnable()
        {
            platformCollider.gameObject.SetActive(true);
            StartCoroutine(FadeIn());
            UpdateLabelHeights();
        }

        public void OnDoDisable()
        {
            platformCollider.gameObject.SetActive(false);
            StartCoroutine(FadeOut());
        }
        public void SetBaseTransform(Transform transform)
        {
            baseTransform = transform;
        }

        private float baseHeight = 0.0f;
        private float minHeight = float.MaxValue;
        private float maxHeight = 0.0f;
        public float BaseHeight { get => baseHeight; set => baseHeight = value; }
        public float MinHeight { get => minHeight; set => minHeight = value; }
        public float MaxHeight { get => maxHeight; set => maxHeight = value; }

        private void UpdateLabelHeights()
        {
            if (baseTransform == null) return;
            baseHeight = baseTransform.position.y;
            for (int i = 0; i < heightLabels.Length; i++)
            {
                var labelHeight = heightLabels[i].transform.position.y - baseHeight;
                if (minHeight > labelHeight) minHeight = labelHeight;
                if (maxHeight < labelHeight) maxHeight = labelHeight;
                labelHeight = Mathf.Floor(labelHeight * 100) / 100;
                heightLabels[i].text = $"{labelHeight} m";
            }
        }

        IEnumerator FadeIn()
        {
            while (canvasGroup.alpha < 1 - float.Epsilon)
            {
                canvasGroup.alpha += Time.deltaTime;
                yield return null;
            }
        }

        IEnumerator FadeOut()
        {
            while (canvasGroup.alpha > float.Epsilon)
            {
                canvasGroup.alpha -= Time.deltaTime;
                yield return null;
            }
            gameObject.SetActive(false);
        }

    }
}
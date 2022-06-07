using System;
using System.Collections;
using ThePotentialJump.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ThePotentialJump.Utilities
{
    public class HeightViewer : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TextMeshProUGUI textMesh;
        [SerializeField] private Vector3 offset;
        [SerializeField] private Transform baseFloor;
        [SerializeField] private float cageHeight = 0.7f;
        [SerializeField] private DropRegion dropRegion;
        [SerializeField] private Camera cam;

        private float floorHeight = 0.0f;
        private void Awake()
        {
            floorHeight = baseFloor.transform.position.y + cageHeight;
            canvasGroup.alpha = 0.0f;
            if (cam == null) cam = Camera.main;
        }
        private void Start()
        {
            if (dropRegion != null)
            {
                dropRegion.Dropped += (o, e) => DeactivateText();
                dropRegion.Entered += (o, e) => ActivateText();
                dropRegion.Exitted += (o, e) => DeactivateText();
                dropRegion.SetMinHeight(floorHeight);
            }
            else
            {
                fadeInCoroutine = StartCoroutine(FadeIn());
                UpdateTextValue();
            }
        }
        Coroutine fadeInCoroutine;
        Coroutine fadeOutCoroutine;
        Coroutine updateText;
        public void ActivateText()
        {
            if (fadeInCoroutine != null) StopCoroutine(fadeInCoroutine);
            if (fadeOutCoroutine != null) StopCoroutine(fadeOutCoroutine);
            if (updateText != null) StopCoroutine(updateText);

            fadeInCoroutine = StartCoroutine(FadeIn());
            updateText = StartCoroutine(UpdateText());
        }
        public void UpdateTextPosition()
        {
            UpdateText();
        }
        public void DeactivateText()
        {
            if (fadeInCoroutine != null) StopCoroutine(fadeInCoroutine);
            if (fadeOutCoroutine != null) StopCoroutine(fadeOutCoroutine);
            if (updateText != null) StopCoroutine(updateText);
            fadeOutCoroutine = StartCoroutine(FadeOut());
        }

        IEnumerator FadeIn()
        {
            while (canvasGroup.alpha < 1.0f)
            {
                canvasGroup.alpha += Time.deltaTime;
                yield return null;
            }
        }
        IEnumerator FadeOut()
        {
            while (canvasGroup.alpha > 0.0f)
            {
                canvasGroup.alpha -= Time.deltaTime;
                yield return null;
            }
        }

        IEnumerator UpdateText()
        {
            while (true)
            {
                RectTransformUtility.ScreenPointToWorldPointInRectangle(
                    transform as RectTransform,
                    Input.mousePosition,
                    cam,
                    out Vector3 mousePosition
                    );
                transform.position = mousePosition + offset;
                UpdateTextValue();
                yield return null;
            }
        }

        public float Height { get; set; }
        private string lastHeightText = "";
        public event EventHandler<HeightChangedEventArgs> HeightChanged;
        private HeightChangedEventArgs heightChangedEventArgs = new HeightChangedEventArgs();
        private void UpdateTextValue()
        {
            Height = transform.position.y - floorHeight;
            Height = Mathf.Floor((Height) * 100.0f) / 100.0f;
            //float inFoot = Height * 3.28f;
            textMesh.text = $"{Height.ToString("F1")} m";// / {(Mathf.Floor((inFoot) * 100.0f) / 100.0f)} ft";
            if(textMesh.text != lastHeightText)
            {
                lastHeightText = textMesh.text;
                heightChangedEventArgs.Height = Height;
                HeightChanged?.Invoke(this, heightChangedEventArgs);
            }

        }
    }

}
using UnityEngine;
using TMPro;
using System;
using ThePotentialJump.Utilities;
using System.Collections;

namespace ThePotentialJump.Gameplay
{
    [Serializable]
    public class EnergyUIController
    {
        [Header("Energy slider parameters")]
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private ExtendedSlider energySlider;
        [SerializeField] private Vector3 sliderOffset;
        [SerializeField] private CanvasGroup canvasGroup;

        public event EventHandler<HoldEnergyEventArgs> ValueChanged;
        public event EventHandler<HoldEnergyEventArgs> ValueFinalized;
        HoldEnergyEventArgs eventArgs = new HoldEnergyEventArgs();

        public void SetupSlider(Vector3 position, float maxValue)
        {
            energySlider.value = 0;
            energySlider.maxValue = maxValue;
            energySlider.minValue = 0;
            energySlider.onValueChanged.RemoveListener(OnSliderValueChanged);
            energySlider.onValueChanged.AddListener(OnSliderValueChanged);
            energySlider.PointerReleased -= OnValueFinalized;
            energySlider.PointerReleased += OnValueFinalized;
            SetPosition(position);
            inputField.onValueChanged.RemoveListener(OnInputValue);
            inputField.onValueChanged.AddListener(OnInputValue);
        }

        public void SetPosition(Vector3 position)
        {
            energySlider.transform.position = sliderOffset + position;
        }

        public IEnumerator Disable()
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
            while (canvasGroup.alpha>float.Epsilon)
            {
                canvasGroup.alpha -= Time.deltaTime * 5.0f;
                yield return null;
            }
        }

        public IEnumerator Enable()
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
            while (canvasGroup.alpha < 1.0f - float.Epsilon)
            {
                canvasGroup.alpha += Time.deltaTime * 5.0f;
                yield return null;
            }
        }

        public void SetUIValues(float value)
        {
            Debug.Log(value);
            energySlider.value = value;
            inputField.text = ((int)value).ToString();
        }
        private void OnSliderValueChanged(float value)
        {
            if (Mathf.Abs(float.Parse(inputField.text) - value) < float.Epsilon * 3) return;
            eventArgs.Value = value;
            inputField.text = ((int)value).ToString();
            ValueChanged?.Invoke(this, eventArgs);
        }

        private void OnValueFinalized(object o, SliderEventArgs e)
        {
            eventArgs.Value = e.Value;
            ValueFinalized?.Invoke(this, eventArgs);
        }

        private void OnInputValue(string inputValue)
        {
            var value = float.Parse(inputValue);
            if (value > energySlider.maxValue)
            {
                value = energySlider.maxValue;
                inputField.text = ((int)value).ToString();
            }
            if (Mathf.Abs(energySlider.value - value) < float.Epsilon * 3) return;
            energySlider.value = value;
            ValueChanged?.Invoke(this, eventArgs);
        }


    }
}
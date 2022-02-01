using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ThePotentialJump.Menus
{
    public class CollectedCarrotViewer : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI sliderText;
        [SerializeField] private int TotalCarrots = 100;
        [SerializeField] private int CollectedCarrots = 0;
        private void Awake()
        {
            if (slider == null)
                slider = GetComponentInChildren<Slider>();
            if (sliderText == null)
                sliderText = slider.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            ResetSlider();
        }
        public void AddCarrots(int carrots)
        {
            CollectedCarrots += carrots;
            ResetSlider();
        }

        public void SetCollectedCarrots(int collectedCarrots)
        {
            CollectedCarrots = collectedCarrots;
            ResetSlider();
        }

        private void ResetSlider()
        {
            sliderText.text = CollectedCarrots + " / " + TotalCarrots;
            slider.value = CollectedCarrots / (TotalCarrots * 1.0f);
        }
    }
}
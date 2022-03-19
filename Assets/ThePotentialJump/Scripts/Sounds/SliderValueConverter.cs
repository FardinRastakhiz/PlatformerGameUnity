using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.Slider;

namespace ThePotentialJump.Sounds
{
    public class SliderValueConverter : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Vector2 targetInterval;
        [Space]
        public SliderEvent SliderValueChanged;

        public void OnSliderChanged()
        {
            var baseValue = slider.value;
            var normalized = (baseValue - slider.minValue) / (slider.maxValue - slider.minValue);
            var targetValue = targetInterval.x + normalized * (targetInterval.y - targetInterval.x);
            SliderValueChanged?.Invoke(targetValue);
        }
    }
}

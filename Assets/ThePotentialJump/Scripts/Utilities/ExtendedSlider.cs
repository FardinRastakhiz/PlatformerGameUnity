using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ThePotentialJump.Utilities
{
    public class ExtendedSlider : Slider
    {
        public event EventHandler<SliderEventArgs> PointerReleased;
        private SliderEventArgs eventArgs = new SliderEventArgs();
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            eventArgs.Value = value;
            PointerReleased?.Invoke(this, eventArgs);
        }
    }
    public class SliderEventArgs : EventArgs
    {
        public float Value { get; set; }
    }
}
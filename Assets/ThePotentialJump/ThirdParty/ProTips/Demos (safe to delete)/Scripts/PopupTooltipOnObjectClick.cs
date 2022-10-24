using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ModelShark
{
    public class PopupTooltipOnObjectClick : MonoBehaviour
    {
        public void OnMouseDown()
        {
            PopupTooltip(gameObject, "This is some tooltip text.\nRandom integer: " + Random.Range(1000,9999), "Continue");
        }

        /// <summary>Triggers a tooltip immediately on the game object specified.</summary>
        /// <param name="onObject">The game object to pop a tooltip over.</param>
        public void PopupTooltip(GameObject onObject, string bodyText, string buttonText)
        {
            // Add the TooltipTrigger component to the object we want to pop a tooltip up for.
            TooltipTrigger tooltipTrigger = onObject.GetComponent<TooltipTrigger>();

            if (tooltipTrigger == null)
                tooltipTrigger = onObject.AddComponent<TooltipTrigger>();

            TooltipStyle tooltipStyle = Resources.Load<TooltipStyle>("CleanSimpleCloseButton");
            tooltipTrigger.tooltipStyle = tooltipStyle;

            // Set the tooltip text and properties.
            tooltipTrigger.SetText("BodyText", bodyText);
            tooltipTrigger.SetText("ButtonText", String.IsNullOrEmpty(buttonText) ? "Continue" : buttonText);
            tooltipTrigger.tipPosition = TipPosition.TopRightCorner;
            tooltipTrigger.maxTextWidth = 300;
            tooltipTrigger.staysOpen = true; // make this a tooltip that stays open...
            tooltipTrigger.isBlocking = true; // ...and is blocking (no other tooltips allowed while this one is active).
    

            tooltipTrigger.isRemotelyActivated = true;
            // Popup the tooltip and give it the object that triggered it (the Canvas in this case).
            tooltipTrigger.Popup(8f, gameObject);
            
        }
    }
}

using ModelShark;
using UnityEngine;

[RequireComponent(typeof(TooltipTrigger))]
public class TriggerColliderTooltip : MonoBehaviour
{
    void OnTriggerEnter(Collider colliderInfo)
    {
        TooltipTrigger tooltipTrigger = gameObject.GetComponent<TooltipTrigger>();

        tooltipTrigger.isRemotelyActivated = true;
        tooltipTrigger.staysOpen = true;

        // Popup the tooltip (Note: duration doesn't matter, since StaysOpen is True)
        tooltipTrigger.Popup(1f, gameObject);
    }

    void OnTriggerExit(Collider colliderInfo)
    {
        TooltipTrigger tooltipTrigger = gameObject.GetComponent<TooltipTrigger>();
        tooltipTrigger.ForceHideTooltip();
    }
}

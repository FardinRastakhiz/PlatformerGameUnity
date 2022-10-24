using ModelShark;
using UnityEngine;

[RequireComponent(typeof(TooltipTrigger))]
public class NamePlateTooltip : MonoBehaviour
{
    public Transform player;
    public float distance = 10f;

    void ShowNamePlate()
    {
        TooltipTrigger tooltipTrigger = gameObject.GetComponent<TooltipTrigger>();

        tooltipTrigger.isRemotelyActivated = true;
        tooltipTrigger.staysOpen = true;
        tooltipTrigger.isBlocking = false;

        // Popup the tooltip (Note: duration doesn't matter, since StaysOpen is True)
        tooltipTrigger.Popup(1f, gameObject);
    }

    void HideNamePlate()
    {
        TooltipTrigger tooltipTrigger = gameObject.GetComponent<TooltipTrigger>();
        tooltipTrigger.ForceHideTooltip();
    }

    void Update()
    {
        if (player == null)
        {
            Debug.Log("You must assign the transform of your player to this script.");
            Destroy(this);
            return;
        }

        float dist = Vector3.Distance(player.position, transform.position);
        if (dist < distance)
            ShowNamePlate();
        else
            HideNamePlate();
    }
}

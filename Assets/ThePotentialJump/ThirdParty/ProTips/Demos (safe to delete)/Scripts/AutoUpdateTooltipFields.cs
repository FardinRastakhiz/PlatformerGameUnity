using ModelShark;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Put this script on a tooltip trigger. When the tooltip is active for that trigger,
/// this script will constantly update <link> fields in the tooltip, based on the IDs of the links
/// and the data fields specified for the link fields.
/// </summary>
[RequireComponent(typeof(TooltipTrigger))]
public class AutoUpdateTooltipFields : MonoBehaviour
{
    [Tooltip("The fields you want to keep updated in your tooltip, along with the source data for the text to update them to. For each of these fields, you need one or more <link> fields in your tooltip text with a matching ID.")]
    public List<UpdateField> fieldsToUpdate;

    private TooltipTrigger tooltipTrigger;

    private void Start()
    {
        tooltipTrigger = gameObject.GetComponent<TooltipTrigger>();

        GameObject tooltipContainer = TooltipManager.Instance.TooltipContainer;
        if (tooltipContainer == null)
        {
            Debug.LogError("Could not find tooltip container. Make sure you have ProTips setup correctly.");
            Destroy(this);
            return;
        }
    }

    private void Update()
    {
        // If the tooltip for our tooltip trigger is not active and visible, don't try to update it.
        if (!tooltipTrigger.Tooltip.GameObject.activeInHierarchy) return;

        foreach (TextField textField in tooltipTrigger.Tooltip.TextFields)
        {
            string text;
#if TEXTMESHPRO_PRESENT
            if (textField.TextTMP != null)
                text = textField.TextTMP.text;
            else
#endif
                text = textField.Text.text;

            if (string.IsNullOrEmpty(text)) continue; // if there is no text in the tooltip, exit.

            foreach (UpdateField field in fieldsToUpdate)
            {
                string pattern = $"<link id=\"{field.Name}\">(.*?)<\\/link>"; // look for a link in the tooltip text with the field name as the ID.
                string replacementTextData = "";
#if TEXTMESHPRO_PRESENT
                replacementTextData = $"<link id=\"{field.Name}\">{field.tmpData.text}</link>";
#else
                replacementTextData = $"<link id=\"{field.Name}\">{field.textData.text}</link>";
#endif
                string replacementText = Regex.Replace(text, pattern, replacementTextData, RegexOptions.None);
                foreach (Match match in Regex.Matches(text, pattern, RegexOptions.IgnoreCase))
                    text = replacementText;
            }

#if TEXTMESHPRO_PRESENT
            if (textField.TextTMP != null)
                textField.TextTMP.text = text;
            else
#endif
                textField.Text.text = text;
        }
    }
}

[Serializable]
public class UpdateField
{
    [Tooltip("The ID of the <link> field in your tooltip text.")]
    public string Name;
    [Tooltip("The Text gameobject you want to use as the data source for this <link> field.")]
    public Text textData;
#if TEXTMESHPRO_PRESENT
    [Tooltip("The TextMeshPro gameobject you want to use as the data source for this <link> field. This one overrides the Text Data field if both are populated.")]
    public TextMeshProUGUI tmpData;
#endif
}
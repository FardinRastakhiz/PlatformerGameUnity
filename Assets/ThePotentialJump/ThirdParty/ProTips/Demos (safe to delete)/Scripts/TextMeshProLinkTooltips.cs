using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if TEXTMESHPRO_PRESENT
using TMPro;
#endif

namespace ModelShark
{
#if TEXTMESHPRO_PRESENT
    [RequireComponent(typeof(TextMeshProUGUI))]
#endif
    public class TextMeshProLinkTooltips : MonoBehaviour
    {
        public bool doesColorChangeOnHover = true;
        public Color hoverColor = new Color(.25f, .5f, 1f);
        public TooltipStyle defaultTooltipStyle;
#if TEXTMESHPRO_PRESENT
        private TextMeshProUGUI pTextMeshPro;
        private Canvas pCanvas;
        private Camera pCamera;
        private int pCurrentLink = -1;
        private List<Color32[]> pOriginalVertexColors = new List<Color32[]>();
        private TooltipTrigger tooltipTrigger;

        private void Start()
        {
            pTextMeshPro = GetComponent<TextMeshProUGUI>();
            pCanvas = GetComponentInParent<Canvas>();
            pCamera = pCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : pCanvas.worldCamera;

            if (defaultTooltipStyle == null)
            {
                Debug.LogError($"No default tooltip style is assigned on gameobject {this.name}.");
                return;
            }

            // Add the TooltipTrigger component to the TextMeshPro text object.
            tooltipTrigger = gameObject.AddComponent<TooltipTrigger>();
            tooltipTrigger.tooltipStyle = defaultTooltipStyle;

            // Set some extra style properties on the tooltip
            tooltipTrigger.minTextWidth = 280;
            tooltipTrigger.maxTextWidth = 400;
            tooltipTrigger.backgroundTint = Color.white;
            tooltipTrigger.tipPosition = TipPosition.MouseTopRightCorner;

            // This tooltip will be activated through code, so turn off the default OnMouseHover behavior.
            tooltipTrigger.isRemotelyActivated = true;
        }

        private void LateUpdate()
        {
            bool isHoveringOver = TMP_TextUtilities.IsIntersectingRectTransform(pTextMeshPro.rectTransform, Input.mousePosition, pCamera);
            int linkIndex = isHoveringOver ? TMP_TextUtilities.FindIntersectingLink(pTextMeshPro, Input.mousePosition, pCamera) : -1;

            // If we ARE NOT hovering over an interactable link...
            if (pCurrentLink != -1 && linkIndex != pCurrentLink)
            {
                // Reset the link color
                if (doesColorChangeOnHover)
                {
                    SetLinkToColor(pCurrentLink, (linkIdx, vertIdx) => pOriginalVertexColors[linkIdx][vertIdx]);
                    pOriginalVertexColors.Clear();
                    pCurrentLink = -1;
                }

                // Close the tooltip
                if (tooltipTrigger != null)
                    tooltipTrigger.Tooltip.Deactivate();
            }

            // If we ARE hovering over an interactable link...
            if (linkIndex != -1 && linkIndex != pCurrentLink)
            {
                // Set the current link index
                pCurrentLink = linkIndex;

                // Change the hover color if applicable
                if (doesColorChangeOnHover)
                    pOriginalVertexColors = SetLinkToColor(linkIndex, (_linkIdx, _vertIdx) => hoverColor);

                // Open the tooltip
                if (defaultTooltipStyle != null && tooltipTrigger != null)
                {
                    LinkData linkData = GetLinkData();
                    if (!string.IsNullOrEmpty(linkData.TooltipStyle))
                    {
                        TooltipStyle tooltipStyle = Resources.Load<TooltipStyle>(linkData.TooltipStyle);
                        if (tooltipStyle != null)
                            tooltipTrigger.tooltipStyle = tooltipStyle;
                        else
                            Debug.LogError($"Could not load tooltip {linkData.TooltipStyle} from Resources folder.");
                    }
                    else // use the default tooltip style if none is passed in on the link data
                        tooltipTrigger.tooltipStyle = defaultTooltipStyle;

                    string tooltipText = LinkDefinitions.GetTextByLinkId(linkData.Id);
                    tooltipTrigger.SetText("BodyText", tooltipText);
                    tooltipTrigger.StartHover();
                }
            }
        }

        private List<Color32[]> SetLinkToColor(int linkIndex, Func<int, int, Color32> colorForLinkAndVert)
        {
            TMP_LinkInfo linkInfo = pTextMeshPro.textInfo.linkInfo[linkIndex];

            var oldVertColors = new List<Color32[]>();

            for (int i = 0; i < linkInfo.linkTextLength; i++)
            {
                int characterIndex = linkInfo.linkTextfirstCharacterIndex + i;
                var charInfo = pTextMeshPro.textInfo.characterInfo[characterIndex];
                int meshIndex = charInfo.materialReferenceIndex;
                int vertexIndex = charInfo.vertexIndex;

                Color32[] vertexColors = pTextMeshPro.textInfo.meshInfo[meshIndex].colors32;
                oldVertColors.Add(vertexColors.ToArray());

                if (!charInfo.isVisible) continue;
                vertexColors[vertexIndex + 0] = colorForLinkAndVert(i, vertexIndex + 0);
                vertexColors[vertexIndex + 1] = colorForLinkAndVert(i, vertexIndex + 1);
                vertexColors[vertexIndex + 2] = colorForLinkAndVert(i, vertexIndex + 2);
                vertexColors[vertexIndex + 3] = colorForLinkAndVert(i, vertexIndex + 3);
            }

            pTextMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.All);

            return oldVertColors;
        }

        private LinkData GetLinkData()
        {
            int linkIdx = TMP_TextUtilities.FindIntersectingLink(pTextMeshPro, Input.mousePosition, pCamera);
            if (linkIdx == -1) return null;

            LinkData linkData = new LinkData();
            linkData.Index = linkIdx;
            string linkId = pTextMeshPro.textInfo.linkInfo[linkIdx].GetLinkID();

            string[] dataParts = linkId.Split('|');
            linkData.Id = dataParts[0];
            if (dataParts.Length > 1)
            {
                linkData.TooltipStyle = dataParts[1];
            }
            return linkData;
        }

        private class LinkData
        {
            public int Index;
            public string Id;
            public string TooltipStyle;
        }
#endif
    }
}
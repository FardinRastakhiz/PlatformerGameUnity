using System.Collections.Generic;

namespace ModelShark
{
    public static class LinkDefinitions
    {
        /// <summary>
        /// This is a collection of link and tooltip data for TextMeshPro links. Each link's display text and tooltip text is contained in this Dictionary.
        /// </summary>
        private static readonly Dictionary<string, string> links = new Dictionary<string, string>
        {
            {
                "linkTag",
                "In your TextMeshPro text, insert a tag such as this:\n\nLearn about ˂link=\"frogs\"˃frogs˂/link˃."
            },
            {
                "linkDefinitions",
                "The LinkDefinitions.cs script is located here:\n\nAssets/ProTips/Demos (safe to delete)/Scripts\n\nCheck out how the LinkDefinitions are configured for each link, and add your own."
            },
            {
                "linkScript",
                "Take a look at the Hierarchy window for this scene and notice how the Canvas/Main Content Area/Text With Links (TMP) gameobject has the TextMeshProLinkTooltips.cs script and how it's configured."
            },
            {
                "pizza",
                "You Chose Pizza!\n\nCheck out the Assets/ProTips/Demos (safe to delete)/Scripts/TextMeshProLinkButtons.cs file to see how this tooltip was configured."
            },
            {
                "waffles",
                "You Chose Waffles!\n\nCheck out the Assets/ProTips/Demos (safe to delete)/Scripts/TextMeshProLinkButtons.cs file to see how this tooltip was configured."
            }
        };

        /// <summary>This is a helper function that allows the caller to fetch the text for a given linkID.</summary>
        /// <param name="linkId">The unique identifier for the link in the LinkDefinition.</param>
        /// <returns>The text contained within the LinkDefinition for the specified linkID.</returns>
        public static string GetTextByLinkId(string linkId)
        {
            if (links.ContainsKey(linkId))
                return links[linkId];

            return "";
        }
    }
}
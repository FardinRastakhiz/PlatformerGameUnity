using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TMPro;
using UnityEngine.Scripting;

namespace ThePotentialJump.Dialogues
{
    public class StriplessStringEnumConverter : StringEnumConverter
    {
        [Preserve]
        public StriplessStringEnumConverter(): base()
        {
            
        }
    }
    public class DialogueSection
    {
        private string id;
        private string speaker;
        private string paragraph;
        private TextAlignmentOptions titleAlignment;

        public string Id { get => id; set => id = value; }
        public string Speaker { get => speaker; set => speaker = value; }
        public string Paragraph { get => paragraph; set => paragraph = value; }
        [JsonConverter(typeof(StriplessStringEnumConverter))]
        public TextAlignmentOptions TitleAlignment { get => titleAlignment; set => titleAlignment = value; }

        public DialogueSection()
        {

        }
    }
}

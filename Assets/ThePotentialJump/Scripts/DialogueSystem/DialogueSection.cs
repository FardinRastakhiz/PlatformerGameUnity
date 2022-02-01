using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TMPro;

namespace ThePotentialJump.Dialogues
{
    public class DialogueSection
    {
        private string speaker;
        private string paragraph;
        private TextAlignmentOptions titleAlignment;

        public string Speaker { get => speaker; set => speaker = value; }
        public string Paragraph { get => paragraph; set => paragraph = value; }
        [JsonConverter(typeof(StringEnumConverter))]
        public TextAlignmentOptions TitleAlignment { get => titleAlignment; set => titleAlignment = value; }
    }
}

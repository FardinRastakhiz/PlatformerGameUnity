using System.Collections.Generic;

namespace ThePotentialJump.Dialogues
{
    public class Dialogue : Dictionary<string, DialogueSection>
    {
        private string name;
        public string Name { get => name; set => name = value; }

        public Dialogue()
        {

        }
    }
}

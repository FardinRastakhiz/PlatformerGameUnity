using SimpleJSON;
using UnityEngine;

namespace ThePotentialJump.Menus
{
    public class EventOccuranceViewer : TextPresenter
    {
        [SerializeField] private string eventName;
        private int totalOccurence = 0;
        public void OnEventHappend()
        {
            totalOccurence++;
            textComponent.text = $"{totalOccurence} {SharedState.LanguageDefs?[id]}";
        }

        public override void AlterLanguage(JSONNode langNode)
        {
            eventName = langNode?[id];
        }
    }

}
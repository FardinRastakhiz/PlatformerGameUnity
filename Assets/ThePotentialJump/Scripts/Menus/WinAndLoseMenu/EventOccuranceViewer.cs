using ThePotentialJump.Gameplay;
using TMPro;
using UnityEngine;

namespace ThePotentialJump.Menus
{
    public class EventOccuranceViewer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMeshPro;
        [SerializeField] private string eventName;
        private int totalOccurence = 0;
        public void OnEventHappend()
        {
            totalOccurence++;
            textMeshPro.text = $"{totalOccurence} {eventName}";
        }
    }

}
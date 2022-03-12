using ThePotentialJump.Utilities;
using UnityEngine;

namespace ThePotentialJump.Menus
{
    public class NewGameButtonClick : MonoBehaviour
    {
        [SerializeField] private SceneFadeInOut sceneFadeInOut;
        public void OnNewGameClicked()
        {
            sceneFadeInOut.FadeOut(0.3f, "Stage1Cutscene");
        }
    }
}
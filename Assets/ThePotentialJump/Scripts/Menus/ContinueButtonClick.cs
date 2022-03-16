using System.Linq;
using ThePotentialJump.ProgressSystem;
using ThePotentialJump.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ThePotentialJump.Menus
{
    public class ContinueButtonClick : MonoBehaviour
    {
        [SerializeField] private SceneFadeInOut sceneFadeInOut;
        [SerializeField] private string[] orderedScenes;
        public void OnContinueClicked()
        {
            if (SaveAndLoad.Instance.Data == null)
            {
                sceneFadeInOut.FadeOut(0.3f, "Stage1Cutscene");
                return;
            }


            for (int i = 0; i < orderedScenes.Length - 1; i++)
            {
                if (orderedScenes[i] == SaveAndLoad.Instance.Data.LastCompletedLevel)
                {
                    sceneFadeInOut.FadeOut(0.3f, orderedScenes[i + 1]);
                    return;
                }
            }
            sceneFadeInOut.FadeOut(0.3f, "Stage1Cutscene");
        }
    }
}
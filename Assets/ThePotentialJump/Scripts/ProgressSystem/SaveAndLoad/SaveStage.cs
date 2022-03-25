using ThePotentialJump.ProgressSystem;
using UnityEngine;

namespace ThePotentialJump.ProgressSystem
{
    public class SaveStage : MonoBehaviour
    {
        public void Save()
        {
            SaveAndLoad.Instance?.SaveProgress();
        }
    }
}
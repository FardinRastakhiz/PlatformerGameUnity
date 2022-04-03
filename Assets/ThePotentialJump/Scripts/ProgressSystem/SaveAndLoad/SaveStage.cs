using ThePotentialJump.ProgressSystem;
using UnityEngine;

namespace ThePotentialJump.ProgressSystem
{
    public class SaveStage : MonoBehaviour
    {
        public void Save()
        {
            if(SaveAndLoad.Instance == null)
            {
                Debug.Log("SaveAndLoad.Instance is null");
            }
            SaveAndLoad.Instance?.SaveProgress();
        }
    }
}
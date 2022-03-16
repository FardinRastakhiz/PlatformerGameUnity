using ThePotentialJump.ProgressSystem;
using UnityEngine;

public class SaveStage : MonoBehaviour
{
    public void Save()
    {
        SaveAndLoad.Instance?.SaveProgress();
    }
}

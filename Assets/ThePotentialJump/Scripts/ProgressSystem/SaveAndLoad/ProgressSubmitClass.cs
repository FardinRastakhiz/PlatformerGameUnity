using LoLSDK;
using ThePotentialJump.Gameplay;
using UnityEngine;

namespace ThePotentialJump.ProgressSystem
{
    public class ProgressSubmitClass : MonoBehaviour
    {
        [SerializeField] private bool submitOnAwake = false;
        [Space]
        [SerializeField] private int progressValue = 0;
        [SerializeField] private int maxProgressValue = 100;

        private void Start()
        {
            if (submitOnAwake) OnSubmitProgress();
        }
        public void OnSubmitProgress()
        {
            LOLSDK.Instance.SubmitProgress(EconomySystem.Instance.TotalCurrency, progressValue, maxProgressValue);
        }
    }
}

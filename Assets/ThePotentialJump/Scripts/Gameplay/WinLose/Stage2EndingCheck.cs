using System.Collections;
using UnityEngine;

namespace ThePotentialJump.Gameplay
{
    public class Stage2EndingCheck : EndingCheck
    {
        [SerializeField] private float delay;
        [SerializeField] private int minCollectToWin = 25;
        public void OnEndTriggered()
        {
            StartCoroutine(StartEnding());
        }
        IEnumerator StartEnding()
        {
            yield return new WaitForSeconds(delay);
            if (EconomySystem.Instance == null)
            {
                Debug.LogError("(Stage2EndingCheck) EconomySystem.Instance cannot be null!");
                yield break;
            }
            var collectedCurrency = EconomySystem.Instance.CollectedOnCurrentScene;
            Debug.Log($"collectedCurrency: {collectedCurrency}, minCollectToWin: {minCollectToWin}");
            if (collectedCurrency >= minCollectToWin)
                Passed?.Invoke();
            else
                Failed?.Invoke();
        }
    }

}

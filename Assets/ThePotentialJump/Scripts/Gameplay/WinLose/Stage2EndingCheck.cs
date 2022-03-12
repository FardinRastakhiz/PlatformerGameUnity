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
            var collectedCurrency = EconomySystem.Instance.CollectedOnCurrentScene;
            if (collectedCurrency >= minCollectToWin)
                Passed?.Invoke();
            else
                Failed?.Invoke();
        }
    }

}

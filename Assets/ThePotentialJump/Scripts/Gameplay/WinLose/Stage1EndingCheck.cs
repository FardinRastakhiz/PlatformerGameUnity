using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ThePotentialJump.Gameplay
{
    public class Stage1EndingCheck : EndingCheck
    {
        [SerializeField] private float delay;
        [SerializeField] private int minCollectToWin = 25;

        //public UnityEvent GameplayPassed;
        //public UnityEvent GameplayFailed;
        public void OnInventoryEmptied()
        {
            StartCoroutine(StartEnding());
        }

        IEnumerator StartEnding()
        {
            yield return new WaitForSeconds(delay);
            var collectedCurrency = EconomySystem.Instance.CollectedOnCurrentScene;
            Debug.Log($"collectedCurrency: {collectedCurrency}");
            if (collectedCurrency >= minCollectToWin)
                Passed?.Invoke();
            else
                Failed?.Invoke();
        }

        //IEnumerator StartEnding()
        //{
        //    yield return new WaitForSeconds(delay);
        //    var collectedCurrency = EconomySystem.Instance.CollectedOnCurrentScene;
        //    if (collectedCurrency >= minCollectToWin)
        //        StartCoroutine(WinTheGame());
        //    else
        //        StartCoroutine(LostTheGame());
        //}

        //public void ActivatePassMenu()
        //{
        //    StartCoroutine(WinTheGame());
        //}

        //public void ActivateFailMenu()
        //{
        //    StartCoroutine(LostTheGame());
        //}

        //IEnumerator WinTheGame()
        //{
        //    yield return new WaitForSeconds(delay);
        //    StagePassed?.Invoke();
        //}
        //IEnumerator LostTheGame()
        //{
        //    yield return new WaitForSeconds(delay);
        //    StageFailed?.Invoke();
        //}
    }
}

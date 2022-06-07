using LoLSDK;
using System.Collections;
using UnityEngine;


namespace ThePotentialJump.Gameplay
{
    public class GameCompleted : MonoBehaviour
    {
        public void Start()
        {
            StartCoroutine(CompleteWithDelay());
        }
        IEnumerator CompleteWithDelay()
        {
            yield return new WaitForSeconds(2);
            LOLSDK.Instance.CompleteGame();
        }
    }
}

using LoLSDK;
using System.Collections;
using UnityEngine;

namespace ThePotentialJump.ProgressSystem
{


    [System.Serializable]
    public class GameProgressData
    {
        public int Score { get; set; }
        public int currentProgress { get; set; }
        public int maximumProgress { get; set; }
    }

    public class SaveGame : Utilities.Singleton<SaveGame>
    {
        [SerializeField] private TMPro.TextMeshProUGUI feedbackText;
        WaitForSeconds _feedbackTimer = new WaitForSeconds(2);
        private GameProgressData data = new GameProgressData();
        Coroutine _feedbackMethod;
        // Start is called before the first frame update
        private void SaveProgress()
        {
            // At least 8 progress point should be submitted
            LOLSDK.Instance.SubmitProgress(100, 100, 100);
            LOLSDK.Instance.SaveState(data);
            LOLSDK.Instance.SaveResultReceived -= OnSaveResult;
        }

        private void GameCompleted()
        {
            LOLSDK.Instance.CompleteGame();
        }

        void OnSaveResult(bool success)
        {
            if (!success)
            {
                Debug.LogWarning("Saving not successful");
                return;
            }

            if (_feedbackMethod != null)
                StopCoroutine(_feedbackMethod);
            // ...Auto Saving Complete
            _feedbackMethod = StartCoroutine(_Feedback("autoSave"));
        }

        public void LoadGameProgress()
        {
            LOLSDK.Instance.LoadState<GameProgressData>(state =>
            {
                if (state != null)
                {
                    data.currentProgress = state.currentProgress;
                    data.Score = state.score;
                    data.maximumProgress = state.maximumProgress;
                }
            });
        }


        IEnumerator _Feedback(string text)
        {
            feedbackText.text = text;
            yield return _feedbackTimer;
            feedbackText.text = string.Empty;
            _feedbackMethod = null;
        }
    }

}
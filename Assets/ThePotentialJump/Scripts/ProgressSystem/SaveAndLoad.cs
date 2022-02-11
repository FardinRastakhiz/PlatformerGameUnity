using LoLSDK;
using System.Collections;
using UnityEngine;

namespace ThePotentialJump.ProgressSystem
{



    public class SaveAndLoad : Utilities.MonoSingleton<SaveAndLoad>
    {
        [SerializeField] private TMPro.TextMeshProUGUI feedbackText;
        WaitForSeconds _feedbackTimer = new WaitForSeconds(2);
        Coroutine _feedbackMethod;
        // Start is called before the first frame update
        private void SaveProgress(GameProgressData data)
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

        public GameProgressData LoadGameProgress()
        {
            GameProgressData data = null;
            LOLSDK.Instance.LoadState<GameProgressData>(state =>
            {
                if (state != null)
                {
                    data = new GameProgressData();
                    data.currentProgress = state.currentProgress;
                    data.Score = state.score;
                    data.maximumProgress = state.maximumProgress;
                }
            });
            return data;
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
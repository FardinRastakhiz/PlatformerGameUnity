using LoLSDK;
using System.Collections;
using ThePotentialJump.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ThePotentialJump.ProgressSystem
{
    public class SaveAndLoad : Utilities.MonoSingleton<SaveAndLoad>
    {

        GameProgressData data;
        WaitForSeconds _feedbackTimer = new WaitForSeconds(2);
        Coroutine _feedbackMethod;

        public GameProgressData Data => data;

        // Start is called before the first frame update

        protected override void Awake()
        {
            base.Awake();
            data = LoadGameProgress();
        }

        public void SaveProgress()
        {
            if (data == null) data = new GameProgressData();
            data.LastCompletedLevel = SceneManager.GetActiveScene().name;
            data.Score = EconomySystem.Instance.TotalCurrency;
            if (!data.CompletedLevels.Contains(data.LastCompletedLevel))
                data.CompletedLevels.Add(data.LastCompletedLevel);
             
            // At least 8 progress point should be submitted
            LOLSDK.Instance.SubmitProgress(
                EconomySystem.Instance.TotalCurrency,
                EconomySystem.Instance.CollectedOnCurrentScene,
                EconomySystem.Instance.MaximumCapacity);

            LOLSDK.Instance.SaveState(data);
            LOLSDK.Instance.SaveResultReceived -= OnSaveResult;
        }

        public void GameCompleted()
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
            //_feedbackMethod = StartCoroutine(_Feedback("autoSave"));
        }

        public GameProgressData LoadGameProgress()
        {
            GameProgressData data = null;
            LOLSDK.Instance.LoadState<GameProgressData>(state =>
            {
                if (state != null)
                {
                    data = state.data;
                    var totalCurrency = EconomySystem.Instance.TotalCurrency;
                    EconomySystem.Instance.Deposit(data.Score - totalCurrency);
                }
            });
            return data;
        }


        //IEnumerator _Feedback(string text)
        //{
        //    feedbackText.text = text;
        //    yield return _feedbackTimer;
        //    feedbackText.text = string.Empty;
        //    _feedbackMethod = null;
        //}
    }

}
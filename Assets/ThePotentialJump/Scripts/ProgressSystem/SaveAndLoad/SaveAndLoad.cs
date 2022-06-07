using LoLSDK;
using System.Collections;
using ThePotentialJump.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        }
        private void Start()
        {
            //data = LoadGameProgress();
            LOLSDK.Instance.SaveResultReceived += OnSaveResult;
        }

        public void SaveProgress()
        {
            if (data == null) data = new GameProgressData();
            data.LastCompletedLevel = SceneManager.GetActiveScene().name;
            data.Score = EconomySystem.Instance.TotalCurrency;
            if (!data.CompletedLevels.Contains(data.LastCompletedLevel))
                data.CompletedLevels.Add(data.LastCompletedLevel);

            //LOLSDK.Instance.SubmitProgress(
            //    EconomySystem.Instance.TotalCurrency,
            //    EconomySystem.Instance.CollectedOnCurrentScene,
            //    EconomySystem.Instance.MaximumCapacity);

            LOLSDK.Instance.SaveState(data);
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

            Debug.Log("Saved successfully");
            //if (_feedbackMethod != null)
            //    StopCoroutine(_feedbackMethod);
            // ...Auto Saving Complete
            //_feedbackMethod = StartCoroutine(_Feedback("autoSave"));
        }
        private void OnDestroy()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                return;
#endif
            LOLSDK.Instance.SaveResultReceived -= OnSaveResult;
        }

        //public GameProgressData LoadGameProgress()
        //{
        //    Debug.Log($"1: {LOLSDK.Instance}");
        //    if (LOLSDK.Instance == null)
        //    {
        //        Debug.LogError("LOLSDK.Instance cannot be null!");
        //        return null;
        //    }
        //    GameProgressData data = null;
        //    Debug.Log($"2: {data}");
        //    LOLSDK.Instance.LoadState<GameProgressData>(state =>
        //    {
        //        Debug.Log($"3: {state}");
        //        if (state != null)
        //        {
        //            data = state.data;
        //            Debug.Log($"4: {data}");
        //            var totalCurrency = EconomySystem.Instance.TotalCurrency;
        //            EconomySystem.Instance.LoadSavedCurrency(data.Score - totalCurrency);
        //            LOLSDK.Instance.SubmitProgress(state.score, state.currentProgress, state.maximumProgress);
        //        }
        //        else
        //        {
        //            Debug.Log("Saved sate is null");
        //        }
        //    });
        //    Debug.Log($"5: {data}");
        //    if (data != null)
        //        Debug.Log($"data: {data.Score},  {data.CompletedLevels},   {data.LastCompletedLevel}");
        //    else
        //        Debug.Log("data is null");
        //    return data;
        //}

        public void StateButtonInitialize<T>(Button newGameButton, Button continueButton, System.Action<T> callback)
            where T : class
        {
            //// Invoke callback with null to use the default serialized values of the state data from the editor.
            //newGameButton.onClick.AddListener(() =>
            //{
            //    newGameButton.gameObject.SetActive(false);
            //    continueButton.gameObject.SetActive(false);
            //    callback(null);
            //});

            // Hide while checking for data.
            newGameButton.gameObject.SetActive(true);
            continueButton.gameObject.SetActive(false);
            // Check for valid state data, from server or fallback local ( PlayerPrefs )
            LOLSDK.Instance.LoadState<T>(state =>
            {
                if (state != null)
                {
                    if (state.data != null)
                    {
                        this.data = state.data as GameProgressData;
                        continueButton.gameObject.SetActive(true);
                        callback(state.data);
                    }
                    // Hook up and show continue only if valid data exists.
                    continueButton.onClick.AddListener(() =>
                    {
                        //newGameButton.gameObject.SetActive(false);
                        //continueButton.gameObject.SetActive(false);
                        //callback(state.data);
                        // Broadcast saved progress back to the teacher app.
                        LOLSDK.Instance.SubmitProgress(state.score, state.currentProgress, state.maximumProgress);
                    });

                }

                newGameButton.gameObject.SetActive(true);
            });
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
using UnityEngine.UI;

namespace LoLSDK
{
    public class Helper
    {
        /// <summary>
        /// Helper to handle your required NEW GAME and CONTINUE buttons.
        /// Stops double clicking of buttons and shows the continue button only when needed.
        /// Also handles broadcasting out the serialized progress back to the teacher app.
        /// <para>NOTE: This is just a helper method, you can implement this flow yourself but it must send Progress when the state loads.</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newGameButton"></param>
        /// <param name="continueButton"></param>
        /// <param name="callback"></param>
        public static void StateButtonInitialize<T>(Button newGameButton, Button continueButton, System.Action<T> callback)
            where T : class
        {
            // Invoke callback with null to use the default serialized values of the state data from the editor.
            newGameButton.onClick.AddListener(() =>
            {
                newGameButton.gameObject.SetActive(false);
                continueButton.gameObject.SetActive(false);
                callback(null);
            });

            // Hide while checking for data.
            newGameButton.gameObject.SetActive(false);
            continueButton.gameObject.SetActive(false);
            // Check for valid state data, from server or fallback local ( PlayerPrefs )
            LOLSDK.Instance.LoadState<T>(state =>
            {
                if (state != null)
                {
                    // Hook up and show continue only if valid data exists.
                    continueButton.onClick.AddListener(() =>
                    {
                        newGameButton.gameObject.SetActive(false);
                        continueButton.gameObject.SetActive(false);
                        callback(state.data);
                        // Broadcast saved progress back to the teacher app.
                        LOLSDK.Instance.SubmitProgress(state.score, state.currentProgress, state.maximumProgress);
                    });

                    continueButton.gameObject.SetActive(true);
                }

                newGameButton.gameObject.SetActive(true);
            });
        }
    }
}
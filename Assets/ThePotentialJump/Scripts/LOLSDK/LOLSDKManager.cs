using System.Collections;
using System.Collections.Generic;
using LoLSDK;
using ThePotentialJump.ProgressSystem;
using SimpleJSON;
using UnityEngine.SceneManagement;
using System;

namespace ThePotentialJump
{

    public class LOLSDKManager : Utilities.MonoSingleton<LOLSDKManager>
    {

        private GameProgressData data = new GameProgressData();
        public GameProgressData Data { get => data; }
        JSONNode _langNode;

        private void Start()
        {
            data = SaveAndLoad.Instance.LoadGameProgress();
            LOLSDK.Instance.LanguageDefsReceived += new LanguageDefsReceivedHandler(LanguageUpdate);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            UpdateTexts();
        }

        void LanguageUpdate(string langJSON)
        {
            if (string.IsNullOrEmpty(langJSON))
                return;

            _langNode = JSON.Parse(langJSON);

            UpdateTexts();
        }


        private void UpdateTexts()
        {
            var texts = FindObjectsOfType<TextPresenter>();
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].AlterLanguage(_langNode);
            }
        }
    }

}
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using UnityEngine;
using System.Runtime.InteropServices;
using SimpleJSON;
using UnityEngine.Networking;
/**
Version 2.0.0 RC-10
*/
namespace LoLSDK
{
#if UNITY_EDITOR
    public interface ILOLSDK_EDITOR : ILOLSDK_EXTENSION
    {
        void SpeakText (string text, Action<AudioClip> onDownloaded, MonoBehaviour owner, string currentLang = "en", string currentTTSLangKey = "en-US");
    }
#endif

    // Needed because original source for V5 dll is missing... 
    public interface ILOLSDK_EXTENSION : ILOLSDK
    {
        void CancelSpeakText();
    }

    public class WebGL : ILOLSDK_EXTENSION
    {
        // *************************************
        // PLUMBING
        // *************************************

        public const string SDK_VERSION = "5.2";

        [DllImport("__Internal")]
        public static extern void _PostWindowMessage(string msgName, string jsonPayload);
        public void PostWindowMessage(string msgName, string jsonPayload)
        {
            Debug.Log("PostWindowMessage " + msgName);

            _PostWindowMessage(msgName, jsonPayload);
        }

        public void LogMessage(string msg)
        {
            JSONObject payload = new JSONObject()
            {
                ["msg"] = msg
            };

            PostWindowMessage("logMessage", payload.ToString());
        }

        // *************************************
        // GAME LIFECYCLE
        // *************************************

        [DllImport("__Internal")]
        private static extern string _GameIsReady(string gameName, string callbackGameObject, string aspectRatio, string resolution, string sdkVersion);
        public void GameIsReady(string gameName, string callbackGameObject, string aspectRatio, string resolution)
        {
            _GameIsReady(gameName, callbackGameObject, aspectRatio, resolution, SDK_VERSION);
        }

        public void CompleteGame()
        {
            JSONObject payload = new JSONObject();
            PostWindowMessage("complete", payload.ToString());
        }

        // *************************************
        // ANSWER SUBMISSION
        // *************************************

        public void SubmitProgress(int score, int currentProgress, int maximumProgress = -1)
        {
            JSONObject payload = new JSONObject()
            {

                ["score"] = score,
                ["currentProgress"] = currentProgress,
                ["maximumProgress"] = maximumProgress
            };

            PostWindowMessage("progress", payload.ToString());
        }

        public void SubmitAnswer(int questionId, int alternativeId)
        {
            JSONObject payload = new JSONObject()
            {
                ["questionId"] = questionId,
                ["alternativeId"] = alternativeId
            };

            PostWindowMessage("answer", payload.ToString());
        }


        // *************************************
        // SPEECH
        // *************************************

        public void SpeakText(string key)
        {
            JSONObject payload = new JSONObject()
            {
                ["key"] = key
            };

            PostWindowMessage("speakText", payload.ToString());
        }

        /// <summary>
        /// Will force tts cancel.
        /// SpeakText will auto cancel but this is useful if you are changing scenes / panels
        /// and the player may have tts running from a previous text entry.
        /// </summary>
        public void CancelSpeakText()
        {
            PostWindowMessage("speakTextCancel", "{}");
        }

        public void SpeakQuestion(int questionId)
        {
            Debug.Log("SpeakQuestion");

            JSONObject payload = new JSONObject()
            {
                ["questionId"] = questionId
            };

            PostWindowMessage("speakQuestion", payload.ToString());
        }

        public void SpeakAlternative(int alternativeId)
        {
            JSONObject payload = new JSONObject()
            {
                ["alternativeId"] = alternativeId
            };

            PostWindowMessage("speakAlternative", payload.ToString());
        }

        public void SpeakQuestionAndAlternatives(int questionId)
        {
            JSONObject payload = new JSONObject()
            {
                ["questionId"] = questionId
            };

            PostWindowMessage("speakQuestionAndAlternatives", payload.ToString());
        }

        public void Error(string msg)
        {
            JSONObject payload = new JSONObject()
            {
                ["msg"] = msg
            };

            PostWindowMessage("error", payload.ToString());
        }

        public void ShowQuestion()
        {
            Debug.Log("ShowQuestion");

            PostWindowMessage("showQuestion", "{}");
        }

        // *************************************
        // PLAY, STOP, and CONFIGURE SOUNDS
        // *************************************

        public void PlaySound(string file, bool background = false, bool loop = false)
        {
            JSONObject payload = new JSONObject()
            {

                ["file"] = file,
                ["background"] = background,
                ["loop"] = loop
            };

            PostWindowMessage("playSound", payload.ToString());
        }

        public void ConfigureSound(float foreground, float background, float fade)
        {
            JSONObject payload = new JSONObject()
            {
                ["foreground"] = foreground,
                ["background"] = background,
                ["fade"] = fade
            };

            PostWindowMessage("configureSound", payload.ToString());
        }

        public void StopSound(string file)
        {
            JSONObject payload = new JSONObject()
            {
                ["file"] = file
            };

            PostWindowMessage("stopSound", payload.ToString());
        }

        public void SaveState(string data)
        {
            PostWindowMessage("saveState", data);
        }

        public void LoadState()
        {
            PostWindowMessage("loadState", "{}");
        }

        public void GetPlayerActivityId()
        {
            PostWindowMessage("getPlayerActivityId", "{}");
        }
    }

#if UNITY_EDITOR
    public class MockWebGL : ILOLSDK_EDITOR
    {

        /* *********************************************
 		 * MOCK Messages for Local Development
		 * *********************************************

		/* *********************************************
 		 * PLUMBING
		 ********************************************** */

        public void PostWindowMessage(string msgName, string jsonPayload)
        {
            Debug.Log("PostWindowMessage: " + msgName);
            Debug.Log("JSON: " + jsonPayload);
        }

        public void LogMessage(string msg)
        {
            Debug.Log("LogMessage");
        }
        /* *********************************************
 		 * ERROR
		 ********************************************** */

        public void sError(string msg)
        {
            Debug.Log("Error");
        }

        /* *********************************************
 		 * GAME LIFECYCLE
		 ********************************************** */

        public void CompleteGame()
        {
            Debug.Log("CompleteGame");
        }

        string _gameName;
        public void GameIsReady(string gameName, string callbackGameObject, string aspectRatio, string resolution)
        {
            _gameName = gameName;
            Debug.Log($"GameIsReady Editor - name: {gameName}, callbackGameObject: {callbackGameObject}");
        }

        /* *********************************************
 		 * SUBMIT PROGRESS AND ANSWERS
		 ********************************************** */

        public void SubmitProgress(int score, int currentProgress, int maximumProgress = -1)
        {
            Debug.Log("SubmitProgress");
        }

        public void SubmitAnswer(int questionId, int alternativeId)
        {
            Debug.Log("SubmitAnswer");
        }


        /* *********************************************
 		 * QUESTIONS
		 ********************************************** */

        public void ShowQuestion()
        {
            Debug.Log("ShowQuestion");
        }
        /* *********************************************
 		 * SPEECH
		 ********************************************** */

        public void SpeakText(string key)
        {
            Debug.Log("SpeakText");
        }

        public void CancelSpeakText()
        {
            Debug.Log("Cancel SpeakText. NOTE: Will not cancel editor testing SpeakText.");
        }

        Coroutine pollyTTSRequest;
        public void SpeakText (string text, Action<AudioClip> onDownloaded, MonoBehaviour owner, string currentLang = "en", string currentTTSLangKey = "en-US")
        {
            if (string.IsNullOrEmpty(text))
                return;

            var regex = new System.Text.RegularExpressions.Regex("<[^>]+>");
            string cleanString = regex.Replace(text, "");

            if (pollyTTSRequest != null)
            {
                owner.StopCoroutine(pollyTTSRequest);
            }

            pollyTTSRequest = owner.StartCoroutine(_PollyTTS(cleanString, currentLang, currentTTSLangKey, onDownloaded));
        }

        IEnumerator _PollyTTS (string text, string currentLang, string currentTTSLangKey, Action<AudioClip> onDownloaded)
        {
            string clipUrl = null;
            var postData = new JSONObject
            {
                ["text"] = text,
                ["lang"] = currentLang,
                ["lang_code"] = currentTTSLangKey,
                ["company_in_editor"] = Application.companyName,
                ["product_in_editor"] = Application.productName,
                ["game_name_in_editor"] = _gameName,
            };

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(postData.ToString());
            using (var dataRequest = UnityWebRequest.Put("https://app.legendsoflearning.com/developers/tts", bytes))
            {
                dataRequest.SetRequestHeader("Content-Type", "application/json");
                dataRequest.method = UnityWebRequest.kHttpVerbPOST;
                yield return dataRequest.SendWebRequest();

                if (!string.IsNullOrEmpty(dataRequest.error))
                {
                    Debug.LogError($"Error: {dataRequest.error}\nGetting polly file: {text} : {currentLang}");
                    yield break;
                }

                var json = JSON.Parse(dataRequest.downloadHandler.text);
                if (!json["success"].AsBool)
                {
                    Debug.LogError("Error: Success FALSE\nGetting polly file: " + text + " : " + currentLang);
                    yield break;
                }

                clipUrl = json["file"];
            }

            if (string.IsNullOrEmpty(clipUrl))
            {
                Debug.LogError("Error: CLIP URL NULL\nGetting polly file: " + text + " : " + currentLang);
                yield break;
            }

            var clipType = clipUrl.EndsWith(".mp3") ? AudioType.MPEG
                : clipUrl.EndsWith(".ogg") ? AudioType.OGGVORBIS
                : clipUrl.EndsWith(".wav") ? AudioType.WAV
                : AudioType.UNKNOWN;

            using (var clipRequest = UnityWebRequestMultimedia.GetAudioClip(clipUrl, clipType))
            {
                yield return clipRequest.SendWebRequest();

                if (!string.IsNullOrEmpty(clipRequest.error))
                {
                    Debug.LogError($"Error: {clipRequest.error} \nGetting polly audio clip: {text} : {currentLang}");
                    yield break;
                }

                onDownloaded?.Invoke(((DownloadHandlerAudioClip)clipRequest.downloadHandler).audioClip);

                Debug.Log("Playing polly tts: " + text);

                pollyTTSRequest = null;
            }
        }

        public void SpeakQuestion(int questionId)
        {
            Debug.Log("SpeakQuestion");
        }

        public void SpeakQuestionAndAlternatives(int questionId)
        {
            Debug.Log("SpeakQuestionAndAlternatives");
        }

        public void SpeakAlternative(int alternativeId)
        {
            Debug.Log("SpeakAlternative");
        }

        /* *********************************************
 		 * SOUND
		 ********************************************** */
        public void ConfigureSound(float a, float b, float c)
        {
            Debug.Log("ConfigureSound");
        }

        public void PlaySound(string path, bool background, bool loop)
        {
            Debug.Log("PlaySound");
        }

        public void StopSound(string path)
        {
            Debug.Log("StopSound");
        }

        public void SaveState(string data)
        {
            Debug.Log("SaveData");
        }

        public void LoadState()
        {
            Debug.Log("LoadData");
        }

        public void GetPlayerActivityId()
        {
            Debug.Log("GetPlayerActivityId");
        }
    }
#endif
}

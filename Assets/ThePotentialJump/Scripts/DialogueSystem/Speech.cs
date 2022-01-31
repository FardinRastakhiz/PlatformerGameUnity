using LoLSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThePotentialJump.DialogueSystem
{
    public class Speech : MonoBehaviour
    {
		AudioSource _ttsAudioSource;

		void Start()
		{
			_ttsAudioSource = gameObject.AddComponent<AudioSource>();
		}


		public void OnSpeakText(string text)
		{
#if UNITY_EDITOR
			// Had to really work around the current implementation of speak text api in SDK 5.
			// This isn't ideal and is cleaned up in SDK 6 to be a unified call for the api.
			// Get the text directly.
			string languageCode = SharedState.StartGameData["languageCode"];
			// Stop any current tts.
			_ttsAudioSource.Stop();
			// Speak the clip of text requested from using this MonoBehaviour as the coroutine owner.
			((ILOLSDK_EDITOR)LOLSDK.Instance.PostMessage).SpeakText(text,
				clip => { _ttsAudioSource.clip = clip; _ttsAudioSource.Play(); },
				this,
				languageCode);
#else
		LOLSDK.Instance.SpeakText(speakTextArgs.text);
#endif
		}

		public void CancelText()
		{
#if UNITY_EDITOR
			_ttsAudioSource.Stop();
#endif
			((ILOLSDK_EXTENSION)LOLSDK.Instance.PostMessage).CancelSpeakText();
		}
	}
}

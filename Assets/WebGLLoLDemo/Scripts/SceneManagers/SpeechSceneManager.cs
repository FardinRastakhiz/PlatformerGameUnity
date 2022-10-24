using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LoLSDK;
using System;
using UnityEngine.SceneManagement;

public class SpeechSceneManager : MonoBehaviour {

	public Button speakText;
	public Button cancelText;
	public Button backButton;

	public InputField speakTextArgs;

	public Text errorMsg;

	AudioSource _ttsAudioSource;

	void Start () {
		speakText.onClick.AddListener(OnClickSpeakText);

		backButton.onClick.AddListener(OnClickBackButton);

		cancelText.onClick.AddListener(CancelText);

		Debug.Log(speakTextArgs.text);

		_ttsAudioSource = gameObject.AddComponent<AudioSource>();
	}

	private void OnClickSpeakText () {
		Debug.Log("aaa");
#if UNITY_EDITOR
		// Had to really work around the current implementation of speak text api in SDK 5.
		// This isn't ideal and is cleaned up in SDK 6 to be a unified call for the api.
		// Get the text directly.
		string languageCode = SharedState.StartGameData["languageCode"];
		string text = SharedState.LanguageDefs[speakTextArgs.text];
		// Stop any current tts.
		_ttsAudioSource.Stop();
		print(languageCode);
		// Speak the clip of text requested from using this MonoBehaviour as the coroutine owner.
		((ILOLSDK_EDITOR)LOLSDK.Instance.PostMessage).SpeakText(text,
            clip => { _ttsAudioSource.clip = clip; _ttsAudioSource.Play(); },
            this,
            languageCode);
#else
		LOLSDK.Instance.SpeakText(speakTextArgs.text);
#endif
	}

	private void CancelText() {
#if UNITY_EDITOR
		_ttsAudioSource.Stop();
#endif
		((ILOLSDK_EXTENSION)LOLSDK.Instance.PostMessage).CancelSpeakText();
	}

	private void OnClickBackButton () {
		SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
	}
}

using LoLSDK;
using System.Collections;
using UnityEngine;

public class SpeechHandler : MonoBehaviour
{
	[SerializeField] private AudioSource _ttsAudioSource;

    public AudioSource TtsAudioSource { get => _ttsAudioSource; set => _ttsAudioSource = value; }

	public void OnClickSpeakText(string text, string languageCode = "en")
	{
	#if UNITY_EDITOR
		// Had to really work around the current implementation of speak text api in SDK 5.
		// This isn't ideal and is cleaned up in SDK 6 to be a unified call for the api.
		// Get the text directly.
		// Stop any current tts.
		_ttsAudioSource.Stop();
		print(languageCode);
		// Speak the clip of text requested from using this MonoBehaviour as the coroutine owner.
		((ILOLSDK_EDITOR)LOLSDK.Instance.PostMessage).SpeakText(text,
			clip => {_ttsAudioSource.clip = clip; _ttsAudioSource.Play(); },
			this,
			languageCode);
	#else
			LOLSDK.Instance.SpeakText(speakTextArgs.text);
	#endif
	}


	IEnumerator CheckAudioPlay()
    {
        while (_ttsAudioSource.isPlaying)
			yield return new WaitForSeconds(0.2f);
    }

	public void CancelText()
	{
	#if UNITY_EDITOR
		_ttsAudioSource.Stop();
	#endif
		((ILOLSDK_EXTENSION)LOLSDK.Instance.PostMessage).CancelSpeakText();
	}
}

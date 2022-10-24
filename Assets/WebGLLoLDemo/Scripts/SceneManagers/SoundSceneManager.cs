using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LoLSDK;

public class SoundSceneManager : MonoBehaviour {

	public Button backButton;

	public Button configure1;
	public Button configure2;

	public Button playSound1;
	public Button playSound2;
	public Button playSound3;
	public Button playCustomSound;

	public Button stopSound1;
	public Button stopSound2;
	public Button stopSound3;

	public Button stopCustomSound;

	string sound1 = "Music/horseyfootage__abandoned.mp3";
	string sound2 = "Music/beep50.mp3";
	string sound3 = "Music/beep34.mp3";

	void Start () {
		backButton.onClick.AddListener(OnClickBack);

		configure1.onClick.AddListener(OnClickConfigure1);
		configure2.onClick.AddListener(OnClickConfigure2);

		playSound1.onClick.AddListener(OnClickPlaySound1);
		playSound2.onClick.AddListener(OnClickPlaySound2);
		playSound3.onClick.AddListener(OnClickPlaySound3);
		playCustomSound.onClick.AddListener(OnClickPlayCustomSound);

		stopSound1.onClick.AddListener(OnClickStopSound1);
		stopSound2.onClick.AddListener(OnClickStopSound2);
		stopSound3.onClick.AddListener(OnClickStopSound3);
		stopCustomSound.onClick.AddListener(OnClickStopCustomSound);
	}

	void Update () {

	}

	private void OnClickBack () {
		SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
	}

	private void OnClickConfigure1 () {
		LOLSDK.Instance.ConfigureSound(1f, 1f, 1f);
	}

	private void OnClickConfigure2 () {
		LOLSDK.Instance.ConfigureSound(0f, 1f, 1f);
	}

	private void OnClickPlaySound1 () {
		LOLSDK.Instance.PlaySound(sound1);
	}

	private void OnClickPlaySound2 () {
		LOLSDK.Instance.PlaySound(sound2);
	}

	private void OnClickPlaySound3 () {
		LOLSDK.Instance.PlaySound(sound3);
	}

	private void OnClickPlayCustomSound () {
		// LOLSDK.Instance.PlaySound(sound3);
	}

	private void OnClickStopSound1 () {
		LOLSDK.Instance.StopSound(sound1);
	}

	private void OnClickStopSound2 () {
		LOLSDK.Instance.StopSound(sound2);
	}

	private void OnClickStopSound3 () {
		LOLSDK.Instance.StopSound(sound3);
	}

	private void OnClickStopCustomSound () {
		// LOLSDK.Instance.PlaySound(sound3);
	}
}

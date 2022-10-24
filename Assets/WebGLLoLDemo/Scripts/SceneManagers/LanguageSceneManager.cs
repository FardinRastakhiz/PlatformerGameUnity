using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SimpleJSON;

public class LanguageSceneManager : MonoBehaviour {

	public Button backButton;

	public Text welcome;
	public Text readyToPlay;
	public Text greatJob;
	public Text onePlusOne;
	public Text pressContinue;

	void Awake () {
		JSONNode defs = SharedState.LanguageDefs;
		welcome.text = defs["welcome"];
		readyToPlay.text = defs["readyToPlay"];
		greatJob.text = defs["greatJob"];
		onePlusOne.text = defs["onePlusOne"];
		pressContinue.text = defs["pressContinue"];

		backButton.onClick.AddListener(OnClickBack);
	}

	private void OnClickBack () {
		SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SimpleJSON;
using LoLSDK;

public class MainMenuManager : MonoBehaviour {
	public Button commandButton;
	public Button questionButton;
	public Button languageButton;
	public Button soundButton;
	public Button speechButton;
	public Button showQuestionButton;

	public Text langDataText;
	public Text questionDataText;
	public Text answerResultText;
	public Text subTitle;


	void Awake () {
		Debug.Log(SharedState.QuestionList);

		if (SharedState.QuestionList != null) {
			questionDataText.text = "loaded.";
		}

		if (SharedState.LanguageDefs != null) {
			langDataText.text = "loaded.";
		}


		languageButton.onClick.AddListener(OnClickLanguage);
		commandButton.onClick.AddListener(OnClickCommands);
		questionButton.onClick.AddListener(OnClickQuestions);
		soundButton.onClick.AddListener(OnClickSound);
		speechButton.onClick.AddListener(OnClickSpeech);
		showQuestionButton.onClick.AddListener (OnClickShowQuestion);

		LOLSDK.Instance.AnswerResultReceived += new AnswerResultReceivedHandler (this.HandleAnswerResult);

		subTitle.text = $"Game SDK Version {WebGL.SDK_VERSION}";
	}

	private void OnClickLanguage() {
		SceneManager.LoadScene("Language", LoadSceneMode.Single);
	}

	private void OnClickCommands () {
		SceneManager.LoadScene("Commands", LoadSceneMode.Single);
	}

	private void OnClickQuestions () {
		SceneManager.LoadScene("Questions", LoadSceneMode.Single);
	}

	private void OnClickSound () {
		SceneManager.LoadScene("Sound", LoadSceneMode.Single);
	}

	private void OnClickSpeech () {
		SceneManager.LoadScene("Speech", LoadSceneMode.Single);
	}

	private void OnClickShowQuestion () {
		LOLSDK.Instance.ShowQuestion();
	}

	private void HandleAnswerResult(string json) {
		JSONNode answerResult = JSON.Parse(json);
		bool isCorrect = answerResult["isCorrect"];
		Debug.Log (isCorrect);

		if (isCorrect) {
			answerResultText.text = "correct";
		} else {
			answerResultText.text = "incorrect";
		}
	}


	// Coming soon: Perfromance Debugging Tool
	// private void OnClickPerformance () {
	// 	SceneManager.LoadScene("Performance", LoadSceneMode.Single);
	// }
}

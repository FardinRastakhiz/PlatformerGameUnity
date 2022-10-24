using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LoLSDK;

public class SubmitAnswer : MonoBehaviour {

	public Button yourButton;

	void Start()
	{
		Button btn = yourButton.GetComponent<Button>();
		btn.onClick.AddListener(OnClick);
	}

	void OnClick()
	{
		MultipleChoiceAnswer alternative = new MultipleChoiceAnswer();
		alternative.questionId = 1.ToString();
		alternative.alternativeId = 2.ToString();
		LOLSDK.Instance.SubmitAnswer(alternative);
	}
}

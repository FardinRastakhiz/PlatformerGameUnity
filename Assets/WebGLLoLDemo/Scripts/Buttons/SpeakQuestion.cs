using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LoLSDK;

[System.Obsolete]
public class SpeakQuestion : MonoBehaviour {

	public Button yourButton;

    void Start()
	{
		Button btn = yourButton.GetComponent<Button>();
		btn.onClick.AddListener(OnClick);
	}

    void OnClick()
	{
		LOLSDK.Instance.SpeakQuestion(100);
	}
}

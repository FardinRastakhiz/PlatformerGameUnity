using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LoLSDK;

public class SubmitProgress : MonoBehaviour {

	public Button yourButton;

	void Start()
	{
		if (yourButton != null) {
			Button btn = yourButton.GetComponent<Button>();
			btn.onClick.AddListener(OnClick);
		}
	}

	void OnClick()
	{
		LOLSDK.Instance.SubmitProgress(100, 100, 100);
	}
}

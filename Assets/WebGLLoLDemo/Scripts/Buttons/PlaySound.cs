using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LoLSDK;

public class PlaySound : MonoBehaviour {

	public Button yourButton;
  public string soundPath;

	void Start()
	{
		Button btn = yourButton.GetComponent<Button>();
		if (btn) { btn.onClick.AddListener(OnClick); }
	}

	void OnClick()
	{
		LOLSDK.Instance.PlaySound (soundPath);
	}
}

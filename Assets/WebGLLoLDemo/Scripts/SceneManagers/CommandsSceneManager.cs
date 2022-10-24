using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LoLSDK;

public class CommandsSceneManager : MonoBehaviour
{

    public Button backButton;

    void Start()
    {
        backButton.onClick.AddListener(OnClickBack);
    }

    private void OnClickBack()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}

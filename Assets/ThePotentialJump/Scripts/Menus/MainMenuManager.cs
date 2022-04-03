using UnityEngine;
using UnityEngine.UI;
using ThePotentialJump.Utilities;
using ThePotentialJump.ProgressSystem;
using LoLSDK;

namespace ThePotentialJump.Menus
{
    public class MainMenuManager : MonoSingleton<MainMenuManager>
    {
        [SerializeField] private Sprite greenButton;
        [SerializeField] private Sprite grayButton;
        [SerializeField] private Sprite yellowButton;
        [Space]
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button continueButton;
        [Space]
        [SerializeField] private Animator fadeOutController;

        [SerializeField] private CurrencyViewer carrotViewer;
        


        protected override void Awake()
        {
            //newGameButton.onClick.AddListener(() => OnNewGameClicked());
            //continueButton.onClick.AddListener(() => OnContinueClicked());
            if (carrotViewer == null) carrotViewer = transform.parent.gameObject.GetComponentInChildren<CurrencyViewer>();
        }

        private void Start()
        {
            DeactivateButton(continueButton);
            ChangeButtonColor(newGameButton, greenButton);
            ChangeButtonColor(continueButton, grayButton);

            SaveAndLoad.Instance.StateButtonInitialize<GameProgressData>
                (newGameButton, continueButton, LoadDataCallback);

            //if (LOLSDKManager.Instance.Data == null)
            //{
            //    DeactivateButton(continueButton);
            //    ChangeButtonColor(newGameButton, greenButton);
            //    ChangeButtonColor(continueButton, grayButton);
            //}
            //else
            //{
            //    ActivateButton(continueButton);
            //    ChangeButtonColor(newGameButton, yellowButton);
            //    ChangeButtonColor(continueButton, greenButton);
            //    carrotViewer.SetCollectedCarrots(LOLSDKManager.Instance.Data.Score);
            //}
        }

        private void LoadDataCallback(GameProgressData data)
        {
            if (data == null) return;
            LOLSDKManager.Instance.AddData(data);
            ActivateButton(continueButton);
            ChangeButtonColor(newGameButton, yellowButton);
            ChangeButtonColor(continueButton, greenButton);
            carrotViewer.SetCollectedCarrots(data.Score);
        }

        //private void OnContinueClicked()
        //{
        //    fadeOutController.SetBool("PlayFadeOut", true);
        //}

        //private void OnNewGameClicked()
        //{
        //    // data = new GameProgressData(); //??
        //    fadeOutController.SetBool("PlayFadeOut", true);
        //}

        private void DeactivateButton(Button button)
        {
            button.interactable = false;
            var canvasGroup = button.gameObject.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0.5f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        private void ActivateButton(Button button)
        {
            button.interactable = true;
            var canvasGroup = button.gameObject.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1.0f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        private void ChangeButtonColor(Button button, Sprite graphics)
        {
            button.gameObject.GetComponent<Image>().sprite = graphics;
        }

    }
}



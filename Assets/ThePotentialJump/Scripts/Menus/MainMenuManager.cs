using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SimpleJSON;
using LoLSDK;
using ThePotentialJump.ProgressSystem;

namespace ThePotentialJump.Menus
{
    public class MainMenuManager : MonoBehaviour
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

        private GameProgressData data = new GameProgressData();

        void Awake()
        {
            Debug.Log("question list: \n" + SharedState.QuestionList);
            newGameButton.onClick.AddListener(() => OnNewGameClicked());
            continueButton.onClick.AddListener(() => OnContinueClicked());
            if (carrotViewer == null) carrotViewer = transform.parent.gameObject.GetComponentInChildren<CurrencyViewer>();
        }

        private void Start()
        {
            data = SaveAndLoad.Instance.LoadGameProgress();
            if (data == null)
            {
                DeactivateButton(continueButton);
                ChangeButtonColor(newGameButton, greenButton);
                ChangeButtonColor(continueButton, grayButton);
            }
            else
            {
                ActivateButton(continueButton);
                ChangeButtonColor(newGameButton, yellowButton);
                ChangeButtonColor(continueButton, greenButton);
                carrotViewer.SetCollectedCarrots(data.Score);
            }
        }

        private void OnContinueClicked()
        {
            fadeOutController.SetBool("PlayFadeOut", true);
        }

        private void OnNewGameClicked()
        {
            data = new GameProgressData();
            fadeOutController.SetBool("PlayFadeOut", true);
        }

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



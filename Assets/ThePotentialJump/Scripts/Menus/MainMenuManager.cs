using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SimpleJSON;
using LoLSDK;

namespace ThePotentialJump.Menus
{
    public class MainMenuManager: MonoBehaviour
    {
        [SerializeField] private Sprite greenButton;
        [SerializeField] private Sprite grayButton;
        [SerializeField] private Sprite yellowButton;
        [Space]
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button continueButton;
        [Space]
        [SerializeField] private Animator fadeOutController;

		public Button commandButton;
		public Button questionButton;
		public Button languageButton;
		public Button soundButton;
		public Button speechButton;
		public Button showQuestionButton;

		public Text answerResultText;

		void Awake()
		{
			Debug.Log("question list: \n" + SharedState.QuestionList);
		}
	}
}



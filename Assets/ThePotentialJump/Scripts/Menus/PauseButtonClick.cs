using System.Collections;
using UnityEngine;

namespace ThePotentialJump.Menus
{
    public class PauseButtonClick : MonoBehaviour
    {
        [SerializeField] private Animator pauseMenuController;
        private bool pauseActivated;


        private void Start()
        {
            StartCoroutine(CheckForInputs());
        }

        IEnumerator CheckForInputs()
        {
            while (true)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                    OnPauseButtonClicked();
                pauseActivated = pauseMenuController.GetBool("ShowPauseMenu");
                yield return null;
            }
        }

        public void OnPauseButtonClicked()
        {
            if (pauseActivated) return;
            pauseMenuController.SetBool("ShowPauseMenu", true);
        }
    }
}
using System.Collections;
using UnityEngine;

namespace ThePotentialJump.Menus
{
    public class ResumeButtonClick : MonoBehaviour
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
                pauseActivated = pauseMenuController.GetBool("ShowPauseMenu");
                if (Input.GetKeyDown(KeyCode.Escape))
                    OnResumeButtonClicked();
                yield return null;
            }
        }

        public void OnResumeButtonClicked()
        {
            if (!pauseActivated) return;
            pauseMenuController.SetBool("ShowPauseMenu", false);
        }
    }
}
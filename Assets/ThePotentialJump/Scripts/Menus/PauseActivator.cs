using System.Collections;
using UnityEngine;

namespace ThePotentialJump.Menus
{
    public class PauseActivator : Utilities.MonoSingleton<PauseActivator>

    {
        [SerializeField] private Animator pauseMenuController;
        private bool pauseActivated;

        protected override void Awake()
        {
            destroyOnLoad = true;
            base.Awake();
        }

        private void Start()
        {
            if (pauseMenuController == null && PauseMenu.Instance != null)
                pauseMenuController = PauseMenu.Instance.gameObject.GetComponent<Animator>();
            StartCoroutine(CheckForInputs());
        }

        IEnumerator CheckForInputs()
        {
            while (true)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                    OnActivatePause();
                pauseActivated = pauseMenuController.GetBool("ShowPauseMenu");
                yield return null;
            }
        }

        public void OnActivatePause()
        {
            Debug.Log("PauseActivation: " + pauseMenuController.name);
            if (pauseActivated) return;
            // SharedState.
            pauseMenuController.SetBool("ShowPauseMenu", true);
        }
    }
}
using System.Collections;
using UnityEngine;

namespace ThePotentialJump.Menus
{
    public class ResumeActivator : Utilities.MonoSingleton<ResumeActivator>
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
            StartCoroutine(CheckForInputs());
        }

        IEnumerator CheckForInputs()
        {
            while (true)
            {
                pauseActivated = pauseMenuController.GetBool("ShowPauseMenu");
                if (Input.GetKeyDown(KeyCode.Escape))
                    OnActivateResume();
                yield return null;
            }
        }

        public void OnActivateResume()
        {
            if (!pauseActivated) return;
            pauseMenuController.SetBool("ShowPauseMenu", false);
        }
    }
}
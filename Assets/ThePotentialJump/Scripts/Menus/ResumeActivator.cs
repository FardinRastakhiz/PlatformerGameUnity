using ThePotentialJump.Inputs;
using UnityEngine;

namespace ThePotentialJump.Menus
{
    public class ResumeActivator : Utilities.MonoSingleton<ResumeActivator>
    {
        [SerializeField] private Animator pauseMenuController;

        protected override void Awake()
        {
            destroyOnLoad = true;
            base.Awake();
        }

        private void Start()
        {
            InputController.Instance.PressEscape += (o, e) => OnActivateResume();
        }

        public void OnActivateResume()
        {
            if (!PauseMenu.Instance.IsActive)
                return;
            Time.timeScale = 1;
            pauseMenuController.SetBool("ShowPauseMenu", false);
        }


    }
}
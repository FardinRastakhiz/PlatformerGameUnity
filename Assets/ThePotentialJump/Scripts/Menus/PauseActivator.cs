using ThePotentialJump.Inputs;
using ThePotentialJump.Utilities;
using UnityEngine;

namespace ThePotentialJump.Menus
{
    public class PauseActivator : Utilities.MonoSingleton<PauseActivator>
    {
        [SerializeField] private Animator pauseMenuController;

        protected override void Awake()
        {
            destroyOnLoad = true;
            base.Awake();
        }

        private void Start()
        {
            if (pauseMenuController == null && PauseMenu.Instance != null)
                pauseMenuController = PauseMenu.Instance.gameObject.GetComponent<Animator>();
            InputController.Instance.PressEscape += (o, e) => OnActivatePause();
        }

        public void OnActivatePause()
        {
            if (SceneFadeInOut.Instance.FadingOut) return;
            if (PauseMenu.Instance.IsActive) return;
            pauseMenuController.SetBool("ShowPauseMenu", true);
        }

    }
}
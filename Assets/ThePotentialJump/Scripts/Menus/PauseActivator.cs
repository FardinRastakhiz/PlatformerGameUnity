using ThePotentialJump.Inputs;
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
            if (PauseMenu.Instance.IsActive) return;
            pauseMenuController.SetBool("ShowPauseMenu", true);
        }

    }
}
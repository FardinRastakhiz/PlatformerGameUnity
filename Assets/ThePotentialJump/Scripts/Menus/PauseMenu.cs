using System.Collections;
using UnityEngine;

namespace ThePotentialJump.Menus
{
    public class PauseMenu : Utilities.MonoSingleton<PauseMenu>
    {
        [SerializeField]
        private CanvasGroup canvasGroup;
        public bool IsActive { get; private set; }
        protected override void Awake()
        {
            destroyOnLoad = true;
            base.Awake();
        }

        private void Start()
        {
            StartCoroutine(UpdateCanvas());
        }

        private bool stopTime = false;
        IEnumerator UpdateCanvas()
        {
            while (true)
            {
                if (IsActive && canvasGroup.alpha < 0.1f)
                {
                    IsActive = false;
                }
                else if (!IsActive && canvasGroup.alpha > 0.1f)
                {
                    IsActive = true;
                }

                if (canvasGroup.alpha == 1 && !stopTime)
                {
                    stopTime = true;
                    Time.timeScale = 0;
                }
                else if(canvasGroup.alpha < 1 && stopTime)
                {
                    stopTime = false;
                }
                yield return null;
            }
        }
    }

}
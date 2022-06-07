using System;
using System.Collections;
using ThePotentialJump.EditorUtilities;
using ThePotentialJump.ProgressSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace ThePotentialJump.Utilities
{
    public class SceneFadeInOut : MonoSingleton<SceneFadeInOut>
    {
        [SerializeField] private float fadeInDuration = 3.0f;
        [SerializeField] private float fadeOutDuration = 3.0f;
        [SerializeField] private Animator coverAnimator;
        [SerializeField] private string nextLevelToLoad;
        [Space]
        [SerializeField] private UnityEvent FadeInCompleted;
        [SerializeField] private UnityEvent FadeOutBegan;

        protected override void Awake()
        {
            destroyOnLoad = true;
            base.Awake();
            if (coverAnimator == null) coverAnimator = GetComponent<Animator>();
        }
        private void Start()
        {
            if (coverAnimator == null)
            {
                Debug.LogError("coverAnimator cannot be null!");
                return;
            }
            coverAnimator.SetFloat("fadeInSpeed", 1.0f / fadeInDuration);
            coverAnimator.SetFloat("fadeOutSpeed", 1.0f / fadeOutDuration);
            FadeIn();
            if (StageManager.Instance != null) StageManager.Instance.StageClosed += OnStageClosed;
        }

        private void OnStageClosed(object sender, EventArgs e)
        {
            coverAnimator.SetBool("fadeIn", true);
        }

        AnimationClipCompleted[] animationClipCompleted;
        public void FadeOut(float delay = 1.0f, string nextLevelToLoad = "")
        {
            if (!string.IsNullOrEmpty(nextLevelToLoad))
                this.nextLevelToLoad = nextLevelToLoad;
            GC.Collect();
            StartCoroutine(FadeOutCoroutine(delay));
        }

        private void FadeIn()
        {
            coverAnimator.SetBool("fadeIn", true);
            animationClipCompleted = coverAnimator.GetBehaviours<AnimationClipCompleted>();

            if (animationClipCompleted != null)
            {
                for (int i = 0; i < animationClipCompleted.Length; i++)
                {
                    animationClipCompleted[i].Completed -= OnFadeOutComplete;
                    animationClipCompleted[i].Completed -= OnFadeInCompleted;
                    animationClipCompleted[i].Completed += OnFadeInCompleted;
                }
            }
        }

        public bool FadingOut { get; set; } = false;
        IEnumerator FadeOutCoroutine(float delay = 0.0f)
        {
            FadingOut = true;
            yield return new WaitForSeconds(delay);
            FadeOutBegan?.Invoke();
            coverAnimator.SetBool("fadeOut", true);
            if (animationClipCompleted == null) animationClipCompleted = coverAnimator.GetBehaviours<AnimationClipCompleted>();
            if (animationClipCompleted != null)
            {
                for (int i = 0; i < animationClipCompleted.Length; i++)
                {
                    animationClipCompleted[i].Completed -= OnFadeInCompleted;
                    animationClipCompleted[i].Completed -= OnFadeOutComplete;
                    animationClipCompleted[i].Completed += OnFadeOutComplete;
                }
            }
        }

        private void OnFadeInCompleted(object o, EventArgs e)
        {
            coverAnimator.SetBool("fadeIn", false);
            FadeInCompleted?.Invoke();
        }


        private void OnFadeOutComplete(object o, EventArgs e)
        {
            if (!string.IsNullOrEmpty(nextLevelToLoad))
                SceneManager.LoadScene(nextLevelToLoad);
        }
    }

}
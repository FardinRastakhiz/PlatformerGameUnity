using System;
using System.Collections;
using ThePotentialJump.ProgressSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace ThePotentialJump.Utilities
{
    public class SceneFadeInOut : MonoBehaviour
    {
        [SerializeField] private float fadeInDuration = 3.0f;
        [SerializeField] private float fadeOutDuration = 3.0f;
        [SerializeField] private Animator coverAnimator;
        [SerializeField] private string nextLevelToLoad;
        [Space]
        [SerializeField] private UnityEvent FadeInCompleted;
        [SerializeField] private UnityEvent FadeOutBegan;

        private void Start()
        {
            if (coverAnimator == null) coverAnimator = GetComponent<Animator>();
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
        IEnumerator FadeOutCoroutine(float delay = 0.0f)
        {
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
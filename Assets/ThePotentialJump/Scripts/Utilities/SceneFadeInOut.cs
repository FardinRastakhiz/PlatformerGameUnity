using System;
using System.Collections;
using ThePotentialJump.ProgressSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ThePotentialJump.Utilities
{
    public class SceneFadeInOut : MonoBehaviour
    {
        [SerializeField] private float fadeInDuration = 3.0f;
        [SerializeField] private float fadeOutDuration = 3.0f;
        [SerializeField] private Animator coverAnimator;
        [SerializeField] private string nextLevelToLoad;

        private void Start()
        {
            if (coverAnimator == null) coverAnimator = GetComponent<Animator>();
            coverAnimator.SetFloat("fadeInSpeed", 1.0f / fadeInDuration);
            coverAnimator.SetFloat("fadeOutSpeed", 1.0f / fadeOutDuration);
            coverAnimator.SetBool("fadeIn", true);
            if (StageManager.Instance != null) StageManager.Instance.StageClosed += OnStageClosed;
        }

        private void OnStageClosed(object sender, EventArgs e)
        {
            coverAnimator.SetBool("fadeIn", true);
        }

        AnimationClipCompleted animationClipCompleted;
        public void FadeOut(float delay = 1.0f)
        {
            StartCoroutine(FadeOutCoroutine(delay));
        }

        IEnumerator FadeOutCoroutine(float delay = 0.0f)
        {
            yield return new WaitForSeconds(delay);
            coverAnimator.SetBool("fadeOut", true);
            animationClipCompleted = coverAnimator.GetBehaviour<AnimationClipCompleted>();
            if (animationClipCompleted != null)
            {
                animationClipCompleted.Completed -= OnFadeOutComplete;
                animationClipCompleted.Completed += OnFadeOutComplete;
            }
        }
        
        private void OnFadeOutComplete(object o, EventArgs e)
        {
            if (!string.IsNullOrEmpty(nextLevelToLoad))
                SceneManager.LoadScene(nextLevelToLoad);
        }
    }

}
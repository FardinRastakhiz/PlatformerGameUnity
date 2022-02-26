using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ThePotentialJump.ProgressSystem
{
    [RequireComponent(typeof(AnimatorControllerParameters))]
    [RequireComponent(typeof(Animator))]
    public class AnimationPlayer : MonoBehaviour
    {
        [SerializeField] private bool PlayOnAwake;

        private Animator animator;
        private AnimatorControllerParameters activeParameters;

        public bool IsPlaying { get; private set; }
        private void Awake()
        {
            activeParameters = GetComponent<AnimatorControllerParameters>();
            animator = GetComponent<Animator>();
        }
        private void Start()
        {
            if (PlayOnAwake) Play();
        }

        public void Play(AnimatorControllerParameters parameters)
        {
            Debug.Log(parameters.name);
            StartCoroutine(OnPlayAnimation(parameters));
        }
        public void Play(float delay = 0.0f)
        {
            if (activeParameters.name == "DroppingWeight")
            {
                Debug.Log(activeParameters.name);
            }
            StartCoroutine(OnPlayAnimation(activeParameters, delay));
        }


        IEnumerator OnPlayAnimation(AnimatorControllerParameters parameters, float delay = 0.0f)
        {
            yield return new WaitForSeconds(delay);
            parameters.SetParameterFor(animator);
            activeParameters.Play(animator);
            IsPlaying = true;
        }
    }

}
using System.Collections;
using UnityEngine;

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
            StartCoroutine(OnPlayAnimation(parameters));
        }
        public void Play(float delay = 0.0f)
        {
            if (activeParameters == null)
            {
                Debug.LogError("activeParameters cannot be null!");
                return;
            }
            if (activeParameters.name == "DroppingWeight")
            {
                Debug.Log(activeParameters.name);
            }
            StartCoroutine(OnPlayAnimation(activeParameters, delay));
        }


        IEnumerator OnPlayAnimation(AnimatorControllerParameters parameters, float delay = 0.0f)
        {
            if (animator == null)
            {
                Debug.LogError("animator cannot be null!");
                yield break;
            }
            yield return new WaitForSeconds(delay);
            parameters.SetParameterFor(animator);
            activeParameters.Play(animator);
            IsPlaying = true;
        }
    }

}
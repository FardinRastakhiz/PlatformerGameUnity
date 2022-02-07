using System;
using System.Collections;
using UnityEngine;

namespace ThePotentialJump.Cameras
{

    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private float moveThreshold = 0.01f;

        private void Awake()
        {
            if (cam == null)
                cam = GetComponent<Camera>();
        }

        private Coroutine moveCoroutine;
        public void MoveCameraTo(Vector3 position, float transitionRate)
        {
            if (moveCoroutine != null)
                StopCoroutine(moveCoroutine);
            moveCoroutine = StartCoroutine(MoveCameraTo_coroutine(position, transitionRate));
        }

        public event EventHandler<MoveCameraEventArgs> CameraMovedToTheTarget;
        private IEnumerator MoveCameraTo_coroutine(Vector3 targetPosition, float transitionRate)
        {
            while ((cam.transform.position - targetPosition).sqrMagnitude > moveThreshold)
            {
                cam.transform.position = Vector3.Lerp(cam.transform.position, targetPosition, 0.3f * transitionRate * Time.deltaTime);
                yield return null;
            }
            CameraMovedToTheTarget?.Invoke(this, new MoveCameraEventArgs { TargetPosition = targetPosition });
        }

        private Coroutine followCoroutine;
        public void StartFollowing(Transform target)
        {
            if (followCoroutine != null)
                StopCoroutine(followCoroutine);
            followCoroutine = StartCoroutine(Following(target));
        }

        private IEnumerator Following(Transform target)
        {
            while (target != null)
            {
                yield return null;
            }
        }

        public void StopFollowing()
        {
            if (followCoroutine != null)
                StopCoroutine(followCoroutine);
        }

    }
}
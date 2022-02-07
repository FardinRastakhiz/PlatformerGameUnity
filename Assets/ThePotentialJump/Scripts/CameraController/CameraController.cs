using System;
using System.Collections;
using UnityEngine;

namespace ThePotentialJump.Cameras
{

    public class CameraController : Utilities.Singleton<CameraController>
    {
        [SerializeField] private Camera cam;
        [SerializeField] private float moveThreshold = 0.01f;
        [Space]
        [Header("Camera follow parameters")]
        [SerializeField] private Transform followTarget;
        public float dampTime = 0.15f;
        public float xOffset = 0;
        public float yOffset = 0;
        [SerializeField] private float horizontalSpeed = 1.0f;
        [SerializeField] private float verticalSpeed = 1.0f;
        [SerializeField] private float margin = 0.1f;
        private Vector3 velocity = Vector3.zero;
        public float m_DampTime = 10f;

        private CameraControlType controlType;
        public void SetCameraControlType(CameraControlType controlType)
        {
            if (this.controlType == controlType) return;
            switch (controlType)
            {
                case CameraControlType.SetOnPosition:
                    break;
                case CameraControlType.SmoothFollow:
                    break;
                case CameraControlType.Animated:
                    break;
                default:
                    break;
            }
            this.controlType = controlType;
        }

        protected override void Awake()
        {
            destroyOnLoad = true;
            base.Awake();
            if (cam == null)
                cam = GetComponent<Camera>();
            CameraMovedToTheTarget += (o, e) => StartFollowing(followTarget);
            waitFixedDeltaTime = new WaitForSeconds(Time.fixedDeltaTime);
        }

        private Coroutine moveCoroutine;
        public void MoveCameraTo(Vector3 position, float transitionRate)
        {
            if (moveCoroutine != null)
                StopCoroutine(moveCoroutine);
            moveCoroutine = StartCoroutine(MoveCameraTo_coroutine(position, transitionRate));
        }

        public event EventHandler<MoveCameraEventArgs> CameraMovedToTheTarget;

        private WaitForSeconds waitFixedDeltaTime;
        private IEnumerator MoveCameraTo_coroutine(Vector3 targetPosition, float transitionRate)
        {
            while ((cam.transform.position - targetPosition).sqrMagnitude > moveThreshold)
            {
                cam.transform.position = Vector3.Lerp(cam.transform.position, targetPosition, 0.3f * transitionRate * Time.fixedDeltaTime);
                yield return waitFixedDeltaTime;
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
                //float targetX = target.position.x + xOffset;
                //float targetY = target.position.y + yOffset;

                //if (Mathf.Abs(transform.position.x - targetX) > margin)
                //    targetX = Mathf.Lerp(transform.position.x, targetX, 1 / m_DampTime * Time.fixedDeltaTime);

                //if (Mathf.Abs(transform.position.y - targetY) > margin)
                //    targetY = Mathf.Lerp(transform.position.y, targetY, m_DampTime * Time.fixedDeltaTime);

                //transform.position = new Vector3(targetX, targetY, transform.position.z);


                if (target)
                {
                    Vector3 point = cam.WorldToViewportPoint(target.position);
                    Vector3 delta = target.position - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
                    Vector3 destination = transform.position + delta;
                    // transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, Time.fixedDeltaTime);
                    transform.position = Vector3.Lerp(transform.position, destination , 0.25f);

                }
                yield return waitFixedDeltaTime;
            }
        }

        public void StopFollowing()
        {
            if (followCoroutine != null)
                StopCoroutine(followCoroutine);
        }

    }

    public enum CameraControlType
    {
        SetOnPosition,
        SmoothFollow,
        Animated
    }
}
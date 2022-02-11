using System;
using System.Collections;
using UnityEngine;

namespace ThePotentialJump.Cameras
{
    [Serializable]
    public class SmoothFollow2D
    {
        private Camera cam;
        private CameraController controller;
        [Space]
        [Header("Camera follow parameters")]
        [SerializeField] private Transform followTarget;
        [SerializeField] private float horizontalSpeed = 1.0f;
        [SerializeField] private float verticalSpeed = 1.0f;
        [SerializeField] private float margin = 0.1f;
        private Vector3 velocity = Vector3.zero;
        public float m_DampTime = 10f;
        public float dampTime = 0.15f;
        public float xOffset = 0;
        public float yOffset = 0;
        public void SetupParameters(Camera camera, CameraController controller)
        {
            cam = camera;
            this.controller = controller;
        }


        private Coroutine followCoroutine;
        public void StartFollowing()
        {
            if (followCoroutine != null)
                controller.StopCoroutine(followCoroutine);
            followCoroutine = controller.StartCoroutine(Following());
        }

        private WaitForSeconds waitFixedDeltaTime;
        private IEnumerator Following()
        {
            while (followTarget != null)
            {
                //float targetX = target.position.x + xOffset;
                //float targetY = target.position.y + yOffset;

                //if (Mathf.Abs(transform.position.x - targetX) > margin)
                //    targetX = Mathf.Lerp(transform.position.x, targetX, 1 / m_DampTime * Time.fixedDeltaTime);

                //if (Mathf.Abs(transform.position.y - targetY) > margin)
                //    targetY = Mathf.Lerp(transform.position.y, targetY, m_DampTime * Time.fixedDeltaTime);

                //transform.position = new Vector3(targetX, targetY, transform.position.z);


                if (followTarget)
                {
                    Vector3 point = cam.WorldToViewportPoint(followTarget.position);
                    Vector3 delta = followTarget.position - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
                    Vector3 destination = cam.transform.position + delta;
                    // transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, Time.fixedDeltaTime);
                    cam.transform.position = Vector3.Lerp(cam.transform.position, destination, 0.25f);

                }
                yield return waitFixedDeltaTime;
            }
        }

        public void StopFollowing()
        {
            if (followCoroutine != null)
                controller.StopCoroutine(followCoroutine);
        }
    }
}
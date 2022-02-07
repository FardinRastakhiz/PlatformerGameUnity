using UnityEngine;

namespace ThePotentialJump.Cameras
{
    public class TestCamera : MonoBehaviour
    {
        [SerializeField] private Transform targetZoomObject;
        [SerializeField] private CameraController camController;

        private void Start()
        {
            camController.MoveCameraTo(targetZoomObject.position + Vector3.back * 5, 2.0f);
        }

    }
}
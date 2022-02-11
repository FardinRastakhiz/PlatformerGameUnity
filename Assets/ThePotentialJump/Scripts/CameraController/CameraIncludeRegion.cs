using System.Collections;
using UnityEngine;

namespace ThePotentialJump.Cameras
{
    public class CameraIncludeRegion
    {
        [SerializeField] private Vector2 padding;
        [SerializeField] private float FOV;

        [SerializeField] private Renderer[] objectsList;
        public void Include(params Renderer[] renderers)
        {

        }

        private void CalculateCameraPosition(Renderer[] renderers)
        {
            //for (int i = 0; i < renderers.Length; i++)
            //{

            //}
            //renderers[0].bounds.min
        }

        IEnumerator TransmitCamera(Vector3 position)
        {
            while (true)
            {
                yield return null;
            }
        }
    }
}
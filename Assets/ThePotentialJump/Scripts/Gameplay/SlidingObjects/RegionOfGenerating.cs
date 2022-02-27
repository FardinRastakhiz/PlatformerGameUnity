using System.Collections;
using UnityEngine;

namespace ThePotentialJump.Gameplay
{
    public class RegionOfGenerating : MonoBehaviour
    {
        [SerializeField] private Transform lowerRegion;
        [SerializeField] private Transform upperRegion;
        [SerializeField] private float slidingSpeed = 1.0f;

        private Vector3 upperBound;
        private Vector3 lowerBound;
        private Vector3 range;

        private Transform slidingTransform;
        private WaitForSeconds waitForFixedUpdate;
        private void Awake()
        {
            upperBound = upperRegion.position;
            lowerBound = lowerRegion.position;
            range = upperBound - lowerBound;
            waitForFixedUpdate = new WaitForSeconds(Time.fixedDeltaTime);
        }

        private void Start()
        {
            StartCoroutine(UpdateSlidingObject());
        }

        private float startTime = 0.0f;
        public T Generate<T>(T objectToInstantial) where T : MonoBehaviour
        {
            startTime = Random.Range(0.0f, Mathf.PI);
            var interpolation = lowerBound + ((Mathf.Sin(Time.realtimeSinceStartup + startTime) + 1) / 2.0f) * range;
            var slidingObject = Instantiate(objectToInstantial, interpolation, Quaternion.identity, this.transform);
            slidingTransform = slidingObject.transform;
            return slidingObject;
        }

        IEnumerator UpdateSlidingObject()
        {
            while (true)
            {
                yield return waitForFixedUpdate;
                if (slidingTransform)
                {
                    slidingTransform.position = lowerBound + ((Mathf.Sin(slidingSpeed * Time.realtimeSinceStartup + startTime) + 1) / 2.0f) * range;
                }
            }
        }
    }

}
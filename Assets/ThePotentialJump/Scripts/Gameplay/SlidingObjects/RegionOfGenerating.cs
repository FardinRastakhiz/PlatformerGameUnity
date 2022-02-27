using UnityEngine;

namespace ThePotentialJump.Gameplay
{
    public class RegionOfGenerating : MonoBehaviour
    {
        [SerializeField] private Transform lowerRegion;
        [SerializeField] private Transform upperRegion;

        private Vector3 upperBound;
        private Vector3 lowerBound;

        private void Awake()
        {
            upperBound = upperRegion.position;
            lowerBound = lowerRegion.position;
        }

        public T Generate<T>(T objectToInstantial) where T : UnityEngine.Object
        {
            var rand = UnityEngine.Random.Range(0.0f, 1.0f);
            var interpolation = lowerBound + rand * (upperBound - lowerBound);
            return Instantiate(objectToInstantial, interpolation, Quaternion.identity, this.transform);
        }
    }

}
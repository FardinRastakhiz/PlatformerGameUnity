using Fallencake.Tools;
using UnityEngine;
using System;

namespace Fallencake
{
    public class CloudSpawner : FCMultiTypeObjectPooler
    {
        [SerializeField]
        float spawnInterval;

        [SerializeField]
        GameObject endPoint;

        [Range(0F, 100.0f)]
        public float _speed = 1;

        [Range(0F, 100.0f)]
        public float _scale = 10;

        [Range(0F, 100.0f)]
        public float _randomizeY = 5;

        //[HideInInspector]
        public bool _prewarm;

        //[HideInInspector]
        public int _prewarmSize = 10;

        private Vector3 startPos;
        private bool toRight;

        void Start()
        {
            startPos = transform.position;
            if (_prewarm)
            {
                Prewarm();
            }
            Invoke("AttemptSpawn", spawnInterval);

        }

        /// <summary>
		/// Spawns a new cloud object, positions and resizes it
		/// </summary>
        /// <returns>The pooled game object.</returns>
		public virtual GameObject SpawnCloud(Vector3 spawnPosition)
        {
            /// get the next cloud in the pool and make sure it's not null
            GameObject nextCloud = GetPooledGameObject();

            // base checks
            if (nextCloud == null) { return null; }
            if (nextCloud.GetComponent<FCPoolableObject>() == null)
            {
                throw new Exception(gameObject.name + " is trying to spawn objects that don't have a PoolableObject component.");
            }

            // position the cloud
            float startY = UnityEngine.Random.Range(spawnPosition.y - _randomizeY, spawnPosition.y + _randomizeY);
            nextCloud.transform.position = new Vector3(spawnPosition.x, startY, nextCloud.transform.position.z);

            // resize the cloud
            float scale = UnityEngine.Random.Range(0.95f * _scale, 1.25f * _scale);
            nextCloud.transform.localScale = new Vector2(scale, scale);

            // activate the cloud
            nextCloud.gameObject.SetActive(true);

            toRight = endPoint.transform.localPosition.x > 0;

            float speed = UnityEngine.Random.Range(0.5f, 1.5f) * (toRight ? _speed : -_speed);
            nextCloud.GetComponent<FloatingCloud>().StartFloating(speed, endPoint.transform.position.x);

            return (nextCloud);
        }

        void AttemptSpawn()
        {
            SpawnCloud(startPos);

            Invoke("AttemptSpawn", spawnInterval);
        }

        void Prewarm()
        {
            for (int i = 0; i < _prewarmSize; i++)
            {
                float areaLength = Mathf.Abs(endPoint.transform.localPosition.x);
                Vector3 spawnPos = startPos + (toRight ? Vector3.right : -Vector3.right) * (areaLength / _prewarmSize) * i;
                SpawnCloud(spawnPos);
            }
        }

    }
}

using System;
using System.Collections.Generic;
using ThePotentialJump.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace ThePotentialJump.Gameplay
{

    public class DeclareRequirements : MonoSingleton<DeclareRequirements>
    {
        [SerializeField] private Transform springSystem;
        [SerializeField] private Projectile2D projectilePrefab;
        [SerializeField] private SpringController springController;
        [SerializeField] private TrackProjectileHeight projectilesTracker;
        [SerializeField] private DropZone[] dropZones;
        [SerializeField] private ProjectileParameters[] projectingWeights;
        [SerializeField] private ActivatePlatformCollider[] platforms;
        [Space]
        [SerializeField] private UnityEvent projectileDestroyed;
        [SerializeField] private UnityEvent projectilesFinished;
        public event EventHandler ProjectilesFinished;
        private List<ProjectileParameters> projectingWeightsList;
        private int dropZonesCount;
        private int weightsCount;
        private DropZone activeZone;
        private Projectile2D activeProjectile;
        protected override void Awake()
        {
            destroyOnLoad = true;
            base.Awake();
            projectingWeightsList = new List<ProjectileParameters>(projectingWeights);
            dropZonesCount = dropZones.Length;
            weightsCount = projectingWeights.Length;
        }

        public void PlayNext()
        {
            OnNext(this, null);
        }

        private void OnNext(object o, EventArgs e)
        {
            if (weightsCount <= 0)
            {
                projectilesFinished?.Invoke();
                ProjectilesFinished?.Invoke(this, null);
                return;
            }
            springController.EnableSpring();
            var nextWeightIndex = UnityEngine.Random.Range(0, weightsCount);

            activeProjectile = Instantiate(projectilePrefab, springController.transform.position + Vector3.up * 2, Quaternion.identity, springSystem);
            activeProjectile.SetSprite(projectingWeightsList[nextWeightIndex].Icon);
            activeProjectile.SetMass(projectingWeightsList[nextWeightIndex].Mass);
            SetPlatformsHitBody(activeProjectile.transform);
            projectingWeightsList.RemoveAt(nextWeightIndex);
            weightsCount = projectingWeightsList.Count;

            activeProjectile.Replace += OnProjectileDestroyed;
            activeProjectile.Replace += projectilesTracker.OnProjectileDestroyed;
            springController.AddProjectile(activeProjectile, true);


            var nextZoneIndex = UnityEngine.Random.Range(0, dropZonesCount);
            if (activeZone != dropZones[nextZoneIndex])
            {
                activeZone?.OnDoDisable();
                activeZone = dropZones[nextZoneIndex];
                activeZone.SetBaseTransform(activeProjectile.transform);
                activeZone.gameObject.SetActive(true);
            }

            StartCoroutine(projectilesTracker.BeginTracking(activeProjectile.transform, activeZone));
        }

        public void SetPlatformsHitBody(Transform targetBody)
        {
            for (int i = 0; i < platforms.Length; i++)
            {
                platforms[i].HitbodyTransform = targetBody;
            }
        }
        private void OnProjectileDestroyed(object sender, ReplaceObjectEventArgs e)
        {
            springController.DisableSpring();
            projectileDestroyed?.Invoke();
        }
    }
}
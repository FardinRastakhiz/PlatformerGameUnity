using System.Collections;
using System.Collections.Generic;
using ThePotentialJump.Utilities;
using UnityEngine;

namespace ThePotentialJump.Gameplay
{
    public class ReleaseParticles : MonoBehaviour
    {
        [SerializeField] private EdgeCollider2D closedCollider;
        [SerializeField] private EdgeCollider2D openCollider;
        [SerializeField] private ParticleSystem rocketParticle;
        [SerializeField] private GameObject particleDirection;

        public void StartReleasing()
        {
            closedCollider.enabled = false;
            openCollider.enabled = true;
            rocketParticle.Play();
            particleDirection.SetActive(true);
        }

    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ThePotentialJump.Gameplay
{
    public class RocketsController : MonoBehaviour
    {
        [SerializeField] private float basePower = 0.4f;
        [SerializeField] private ParticleSystem[] rocketParticles;
        [SerializeField] private RocketSoundPitchController rocketSoundPitch;

        public UnityEvent rocketsTurnedOn;

        public void TurnOnRockets()
        {
            for (int i = 0; i < rocketParticles.Length; i++)
                rocketParticles[i].Play();
            UpdateRocketPower(0);
            rocketsTurnedOn?.Invoke();
        }

        public void UpdateRocketPower(float power)
        {
            for (int i = 0; i < rocketParticles.Length; i++)
            {
                var main = rocketParticles[i].main;
                main.startLifetime = basePower + power;
            }
            rocketSoundPitch.OnRocketPowerChanged(basePower + power);
        }

    }
}
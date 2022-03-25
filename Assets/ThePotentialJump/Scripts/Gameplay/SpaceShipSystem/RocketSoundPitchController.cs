using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThePotentialJump.Gameplay
{
    public class RocketSoundPitchController : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private float basePowerPitch = 1.0f;
        [SerializeField] private Vector2 interval = new Vector2(1.0f, 2.8f);

        public void OnRocketPowerChanged(float rocketPower)
        {
            var pitch = interval.x + rocketPower / basePowerPitch;
            audioSource.pitch = pitch > interval.y ? interval.y : pitch;
        }
    }
}
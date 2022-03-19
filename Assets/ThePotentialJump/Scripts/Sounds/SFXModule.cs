using System;
using UnityEngine;

namespace ThePotentialJump.Sounds
{
    [RequireComponent(typeof(AudioSource))]
    public class SFXModule : AudioModule
    {
        protected override void Start()
        {
            base.Start();
            SFXVolume.Instance.Muted += OnMute;
            SFXVolume.Instance.UnMuted += OnUnMute;
            SFXVolume.Instance.VolumeChanged += OnVolumeChange;
        }

        public void PlayImmediate(float volume)
        {
            if (Mathf.Abs(this.volume - volume) >= 0.05f * maxVolume)
            {
                this.volume = volume;
                audioSource.volume = volume * maxVolume;
            }
            audioSource.Play();
        }
        public void PlayImmediate()
        {
            audioSource.Play();
        }
    }
}

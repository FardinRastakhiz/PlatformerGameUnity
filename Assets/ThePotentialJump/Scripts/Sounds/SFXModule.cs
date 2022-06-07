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
            if (SFXVolume.Instance == null)
            {
                Debug.LogError("SFXVolume.Instance cannot be null!");
                return;
            }
            if (SFXVolume.Instance.IsMute)
                OnMute(this, null);
            OnVolumeChange(this, new VolumeEventArgs { Volume = SFXVolume.Instance.Volume });
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
            if (audioSource.isPlaying) return;
            audioSource.Play();
        }
        public void PlayOverrideImmediate()
        {
            audioSource.Play();
        }
    }
}

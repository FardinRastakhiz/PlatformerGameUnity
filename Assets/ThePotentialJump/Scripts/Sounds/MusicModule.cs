using System;
using UnityEngine;

namespace ThePotentialJump.Sounds
{
    public class MusicModule : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip clip;

        private void Awake()
        {
            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();
            audioSource.clip = clip;
            MusicVolume.Muted += OnMute;
            MusicVolume.UnMuted += OnUnMute;
            MusicVolume.VolumeChanged += OnVolumeChange;
        }

        private void OnMute(object o, EventArgs e)
        {
            audioSource.mute = true;
        }

        private void OnUnMute(object o, EventArgs e)
        {
            audioSource.mute = false;
        }

        public void OnVolumeChange(object o, VolumeEventArgs e)
        {
            audioSource.volume = e.Volume;
        }
    }

}

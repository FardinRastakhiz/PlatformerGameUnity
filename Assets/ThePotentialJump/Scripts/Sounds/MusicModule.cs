using System;
using System.Collections;
using UnityEngine;

namespace ThePotentialJump.Sounds
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicModule : AudioModule
    {
        protected override void Start()
        {
            base.Start();
            if(MusicVolume.Instance == null)
            {
                Debug.LogError("MusicVolume.Instance cannot be null!");
                return;
            }
            MusicVolume.Instance.Muted += OnMute;
            MusicVolume.Instance.UnMuted += OnUnMute;
            MusicVolume.Instance.VolumeChanged += OnVolumeChange;
        }
    }

}

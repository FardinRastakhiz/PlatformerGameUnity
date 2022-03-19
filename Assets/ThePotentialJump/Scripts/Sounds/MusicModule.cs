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
            MusicVolume.Instance.Muted += OnMute;
            MusicVolume.Instance.UnMuted += OnUnMute;
            MusicVolume.Instance.VolumeChanged += OnVolumeChange;
        }
    }

}

using System;

namespace ThePotentialJump.Sounds
{

    public class SFXVolume : audioVolume
    {
        public static event EventHandler Muted;
        public static event EventHandler UnMuted;
        public static event EventHandler<VolumeEventArgs> VolumeChanged;

        protected override void Mute()
        {
            Muted?.Invoke(this, null);
        }

        protected override void UnMute()
        {
            UnMuted?.Invoke(this, null);
        }

        protected override void ChangeVolume()
        {
            VolumeChanged?.Invoke(this, volumeEventArgs);
        }
    }
}

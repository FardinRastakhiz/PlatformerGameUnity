using System;

namespace ThePotentialJump.Sounds
{

    public class SFXVolume : audioVolume<SFXVolume, SFXModule>
    {
        public event EventHandler Muted;
        public event EventHandler UnMuted;
        public event EventHandler<VolumeEventArgs> VolumeChanged;


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

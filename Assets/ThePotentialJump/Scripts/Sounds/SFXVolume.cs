using System;

namespace ThePotentialJump.Sounds
{

    public class SFXVolume : audioVolume<SFXVolume, SFXModule>
    {
        public event EventHandler Muted;
        public event EventHandler UnMuted;
        public event EventHandler<VolumeEventArgs> VolumeChanged;


        public override void Mute()
        {
            Muted?.Invoke(this, null);
        }

        public override void UnMute()
        {
            UnMuted?.Invoke(this, null);
        }

        public override void ChangeVolume()
        {
            VolumeChanged?.Invoke(this, volumeEventArgs);
        }
    }
}

using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ThePotentialJump.Sounds
{
    public abstract class audioVolume : MonoBehaviour
    {
        private bool isMute;
        [SerializeField] private Button muteButton;
        [SerializeField] private Slider volumeSlider;

        [SerializeField] private Sprite muteIcon;
        [SerializeField] private Sprite unMuteIcon;

        [SerializeField] private Image iconImage;


        protected VolumeEventArgs volumeEventArgs = new VolumeEventArgs();
        private void Awake()
        {
            iconImage = muteButton.gameObject.GetComponentsInChildren<Image>()
                .Where(i => i.gameObject != muteButton.gameObject).FirstOrDefault();
            iconImage.sprite = unMuteIcon;
            muteButton.onClick.AddListener(() => OnMuteClicked());
            volumeSlider.onValueChanged.AddListener(c =>
            {
                volumeEventArgs.Volume = c;
                ChangeVolume();
            });
        }

        private void OnMuteClicked()
        {
            isMute = !isMute;
            if (isMute)
            {
                iconImage.sprite = muteIcon;
                Mute();
            }
            else
            {
                iconImage.sprite = unMuteIcon;
                UnMute();
            }
        }

        protected abstract void Mute();

        protected abstract void UnMute();

        protected abstract void ChangeVolume();
    }
}

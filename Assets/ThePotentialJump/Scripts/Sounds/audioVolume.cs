using System.Linq;
using ThePotentialJump.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace ThePotentialJump.Sounds
{
    public abstract class audioVolume<T1, T2> : MonoSingleton<T1>
        where T1 : audioVolume<T1, T2>
        where T2 : AudioModule
    {
        protected bool isMute;
        [SerializeField] private Button muteButton;
        [SerializeField] protected Slider volumeSlider;

        [SerializeField] private Sprite muteIcon;
        [SerializeField] private Sprite unMuteIcon;

        [SerializeField] private Image iconImage;
        [SerializeField] private int changeSteps = 17;

        public float Volume => volumeSlider.value / (changeSteps * 1.0f);
        public bool IsMute => isMute;

        protected VolumeEventArgs volumeEventArgs = new VolumeEventArgs();
        protected override void Awake()
        {
            destroyOnLoad = true;
            base.Awake();
            iconImage = muteButton.gameObject.GetComponentsInChildren<Image>()
                .Where(i => i.gameObject != muteButton.gameObject).FirstOrDefault();
            iconImage.sprite = unMuteIcon;
            muteButton.onClick.AddListener(() => OnMuteClicked());
            volumeSlider.onValueChanged.AddListener(c =>
            {
                volumeEventArgs.Volume = c / changeSteps;
                ChangeVolume();
            });
            SetupModules();
        }
        public void ChangeSettings(bool isMute, float volume)
        {
            this.isMute = !isMute;
            volumeEventArgs.Volume = volume;
            volumeSlider.value = volume * changeSteps;
            //ChangeVolume();
            OnMuteClicked();
            SetupModules();
        }
        private void SetupModules()
        {
            var modules = FindObjectsOfType<T2>();
            for (int i = 0; i < modules.Length; i++)
                modules[i].SetupSettings(isMute, volumeSlider.value / changeSteps);
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

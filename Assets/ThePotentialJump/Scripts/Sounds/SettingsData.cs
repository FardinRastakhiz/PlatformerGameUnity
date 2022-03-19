using System;
using ThePotentialJump.Sounds;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ThePotentialJump.Utilities
{
    public class SettingsData : MonoSingleton<SettingsData>
    {
        private SFXVolume sfxVolumeClass;
        private MusicVolume musicVolumeClass;
        private float sfxVolume;
        private float musicVolume;
        private bool sfxMute;
        private bool musicMute;

        private void Start()
        {
            InitializeParameters();
            sfxVolume = sfxVolumeClass.Volume;
            musicVolume = musicVolumeClass.Volume;
            sfxMute = sfxVolumeClass.IsMute;
            musicMute = musicVolumeClass.IsMute;
            Debug.Log($"sfxVolume: {sfxVolume}");
            Debug.Log($"musicVolume: {musicVolume}");
            Debug.Log($"sfxMute: {sfxMute}");
            Debug.Log($"musicMute: {musicMute}");
            SceneManager.sceneLoaded += OnSceneLoaded;
        }


        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            InitializeParameters();
            sfxVolumeClass.ChangeSettings(sfxMute, sfxVolume);
            musicVolumeClass.ChangeSettings(musicMute, musicVolume);
        }

        private void InitializeParameters()
        {
            sfxVolumeClass = FindObjectOfType<SFXVolume>();
            musicVolumeClass = FindObjectOfType<MusicVolume>();

            MusicVolume.Instance.Muted -= OnMusicMute;
            MusicVolume.Instance.UnMuted -= OnMusicUnMute;
            MusicVolume.Instance.VolumeChanged -= OnMusicVolumeChange;
            SFXVolume.Instance.Muted -= OnSFXMute;
            SFXVolume.Instance.UnMuted -= OnSFXUnMute;
            SFXVolume.Instance.VolumeChanged -= OnSFXVolumeChange;

            MusicVolume.Instance.Muted += OnMusicMute;
            MusicVolume.Instance.UnMuted += OnMusicUnMute;
            MusicVolume.Instance.VolumeChanged += OnMusicVolumeChange;
            SFXVolume.Instance.Muted += OnSFXMute;
            SFXVolume.Instance.UnMuted += OnSFXUnMute;
            SFXVolume.Instance.VolumeChanged += OnSFXVolumeChange;
        }

        private void OnMusicMute(object sender, EventArgs e)
        {
            musicMute = true;
        }
        private void OnMusicUnMute(object sender, EventArgs e)
        {
            musicMute = false;
        }
        private void OnMusicVolumeChange(object sender, VolumeEventArgs e)
        {
            Debug.Log($"musicVolume: {musicVolume}");
            musicVolume = e.Volume;
        }

        private void OnSFXMute(object sender, EventArgs e)
        {
            sfxMute = true;
        }
        private void OnSFXUnMute(object sender, EventArgs e)
        {
            sfxMute = false;
        }
        private void OnSFXVolumeChange(object sender, VolumeEventArgs e)
        {
            Debug.Log($"sfxVolume: {sfxVolume}");
            sfxVolume = e.Volume;
        }
    }
}
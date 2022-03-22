using System;
using System.Collections;
using UnityEngine;

namespace ThePotentialJump.Sounds
{
    public class AudioModule : MonoBehaviour
    {
        [SerializeField] protected AudioSource audioSource;
        [SerializeField] private AudioClip clip;
        [Space]
        [SerializeField] protected float maxVolume = 1.0f;
        [SerializeField] private bool playOnAwake = false;
        [SerializeField] private float fadeDuration = 2.0f;
        [Space]
        [SerializeField] private Vector2 trim;

        protected float volume = 0.0f;
        protected bool isMute = false;

        public float MaxVolume => maxVolume;

        protected virtual void Awake()
        {
            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();

            audioSource.clip = TrimClip();
        }

        private AudioClip TrimClip()
        {
            if (trim == Vector2.zero) return clip;
            if (trim.y <= trim.x) trim.y = clip.length;
            return MakeSubclip(clip, trim.x, trim.y);
        }

        protected virtual void Start()
        {
            if (playOnAwake)
                Play();
        }

        public void SetupSettings(bool isMute, float volume)
        {
            this.isMute = isMute;
            audioSource.mute = isMute;
            this.volume = volume;
            audioSource.volume = volume * maxVolume;
        }
        private Coroutine playCoroutine;
        private Coroutine stopCoroutine;
        public void Play()
        {
            if (playCoroutine != null) StopCoroutine(playCoroutine);
            if (stopCoroutine != null) StopCoroutine(stopCoroutine);
            playCoroutine = StartCoroutine(PlayCoroutine());
        }

        public void Stop()
        {
            if (playCoroutine != null) StopCoroutine(playCoroutine);
            if (stopCoroutine != null) StopCoroutine(stopCoroutine);
            stopCoroutine = StartCoroutine(StopCoroutine());
        }

        public void ChangeMaxVolume(float newMaxVolume)
        {
            StartCoroutine(UpdateVolume(newMaxVolume));
        }
        IEnumerator UpdateVolume(float newMaxVolume)
        {
            var sign = Mathf.Sign(maxVolume - newMaxVolume);
            while (sign*(maxVolume - newMaxVolume) > float.Epsilon*2)
            {
                Debug.Log("Update Update");
                maxVolume -= sign * Time.deltaTime / fadeDuration;
                audioSource.volume = volume * maxVolume;
                yield return null;
            }
        }

        IEnumerator PlayCoroutine()
        {
            var volFactor = 0.0f;
            audioSource.Play();
            while (volFactor < 1.0f)
            {
                volFactor += Time.deltaTime / fadeDuration;
                audioSource.volume = volume * volFactor * maxVolume;
                yield return null;
            }
        }
        IEnumerator StopCoroutine()
        {
            var volFactor = 1.0f;
            while (volFactor > 0.0f)
            {
                volFactor -= Time.deltaTime / fadeDuration;
                audioSource.volume = volume * volFactor * maxVolume;
                yield return null;
            }
            audioSource.Stop();
        }

        protected IEnumerator PlayTrimmed()
        {
            if (trim == Vector2.zero)
            {
                audioSource.Play();
                yield break;
            }
            bool finished = false;
            while (!finished)
            {
                yield return null;
            }
        }

        protected void OnMute(object o, EventArgs e)
        {
            audioSource.mute = true;
            isMute = true;
        }

        protected void OnUnMute(object o, EventArgs e)
        {
            audioSource.mute = false;
            isMute = false;
        }

        protected void OnVolumeChange(object o, VolumeEventArgs e)
        {
            volume = e.Volume;
            audioSource.volume = volume * maxVolume;
        }

        /**
        * Creates a sub clip from an audio clip based off of the start time
        * and the stop time. The new clip will have the same frequency as
        * the original.
        */
        private AudioClip MakeSubclip(AudioClip clip, float start, float stop)
        {
            /* Create a new audio clip */
            int frequency = clip.frequency;
            float timeLength = stop - start;
            int samplesLength = (int)(frequency * timeLength);
            AudioClip newClip = AudioClip.Create(clip.name + "-sub", samplesLength, 1, frequency, false);
            /* Create a temporary buffer for the samples */
            float[] data = new float[samplesLength];
            /* Get the data from the original clip */
            clip.GetData(data, (int)(frequency * start));
            /* Transfer the data to the new clip */
            newClip.SetData(data, 0);
            /* Return the sub clip */
            return newClip;
        }
    }



}

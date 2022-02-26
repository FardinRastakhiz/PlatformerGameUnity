using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ThePotentialJump.Dialogues
{
    public class DialogueSectionPlayer : MonoBehaviour
    {
        [SerializeField] private bool isPlaying;
        [SerializeField] private string section;
        [SerializeField] private float endingPause;
        private bool played = false;
        public string Section { get => section; set => section = value; }
        public bool Played { get => played; set => played = value; }
        public bool IsPlaying { get => isPlaying; set => isPlaying = value; }
        public float EndingPause { get => endingPause; set => endingPause = value; }

        public UnityEvent DialogueSectionStarted;
        public void Play(string stage, float overallBeginPause = 0.0f, float overallEndingPause = 0.0f)
        {
            IsPlaying = true;
            DialogueSystem.Instance.OnPlayDialogueSection(stage, Section, Finish, overallBeginPause, EndingPause + overallEndingPause);
            DialogueSectionStarted?.Invoke();
        }


        public UnityEvent DialogueSectionFinished;
        public void Finish()
        {
            Played = true;
            DialogueSectionFinished?.Invoke(); 
        }
    }
}

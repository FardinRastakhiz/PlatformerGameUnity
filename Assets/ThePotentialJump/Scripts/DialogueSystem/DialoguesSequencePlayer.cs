using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ThePotentialJump.Dialogues
{
    public class DialoguesSequencePlayer : MonoBehaviour
    {
        [SerializeField] private string stage;
        [SerializeField] private bool hideDialogueBox;
        [Space]
        [SerializeField] private DialogueSectionPlayer[] sectionPlayers;

        [Space]
        [SerializeField] private float beginPause = 1.0f;
        [SerializeField] private float intervalPause = 1.0f;
        [SerializeField] private float endingPause = 2.0f;

        private bool isDialogueHidden = true;


        public UnityEvent DialoguesSequenceStarted;
        public void Play()
        {
            StartCoroutine(PlaySequence());
            DialoguesSequenceStarted?.Invoke();
        }

        public UnityEvent DialoguesSequencePaused;
        public void Pause()
        {
            DialoguesSequencePaused?.Invoke();
        }

        public UnityEvent DialoguesSequenceFinished;
        public void Finish()
        {
            DialoguesSequenceFinished?.Invoke();
        }

        IEnumerator PlaySequence()
        {
            int activeSection = 0;
            while (activeSection < sectionPlayers.Length)
            {
                if (!sectionPlayers[activeSection].IsPlaying && !sectionPlayers[activeSection].Played)
                {
                    sectionPlayers[activeSection].Play(stage, activeSection == 0 ? beginPause : 0.0f,
                        activeSection == sectionPlayers.Length - 1 ? endingPause : 0.0f);
                    isDialogueHidden = false;
                }
                if (sectionPlayers[activeSection].IsPlaying && sectionPlayers[activeSection].Played)
                {
                    sectionPlayers[activeSection].IsPlaying = false;
                    activeSection++;
                }

                if (hideDialogueBox && !isDialogueHidden)
                {
                    DialogueSystem.Instance.OnCloseButtonClicked();
                }
                yield return null;
            }
        }
    }
}

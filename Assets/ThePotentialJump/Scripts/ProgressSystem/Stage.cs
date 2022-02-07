using System;
using System.Collections.Generic;
using ThePotentialJump.Cameras;
using UnityEngine;

namespace ThePotentialJump.ProgressSystem
{
    [Serializable]
    public class Stage
    {
        [SerializeField] private string name;
        [SerializeField]
        private int number;
        [SerializeField]
        private SeqPart[] parts;
        public int Number { get => number; set => number = value; }
        public Dictionary<string, SeqPart> partsDict = new Dictionary<string, SeqPart>();

        public void SetUpStage()
        {
            for (int i = 0; i < parts.Length; i++)
                partsDict.Add(parts[i].Name, parts[i]);
        }

        public bool IsActive { get; set; }

        public void Add(SeqPart part)
        {
            partsDict.Add(part.Name, part);
        }

        public void PlaySequence(string sequence)
        {
            partsDict[sequence].Play();
        }

        public void StopAllSequences()
        {
            foreach (var seq in partsDict.Values)
            {
                seq.Stop();
            }
        }

        public void StopSequence(string sequence)
        {
            partsDict[sequence].Stop();
        }
    }
}

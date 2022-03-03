using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ThePotentialJump.Gameplay
{
    public class SlidingCagesManager : Replacable
    {
        [SerializeField] private SlidingObject slidingObjectPrefab;
        [SerializeField] private RegionOfGenerating[] regionsOfGenerating;
        [SerializeField] private float generateDelay = 0.33f;
        private Coroutine generateCoroutine;
        private SlidingObject[] slidingObjects;

        public override event EventHandler<ReplaceObjectEventArgs> Replace;
        public UnityEvent CageOpened;
        public UnityEvent CageBroke;

        private WaitForSeconds waitForGenerateDelay;
        private void Awake()
        {
            waitForGenerateDelay = new WaitForSeconds(generateDelay);
            slidingObjects = new SlidingObject[regionsOfGenerating.Length];
        }

        private void Start()
        {
            StartGenerating();
        }
        public void StartGenerating()
        {
            StopGenerating();
            generateCoroutine = StartCoroutine(Generate());
        }

        public void StopGenerating()
        {
            if (generateCoroutine != null) StopCoroutine(generateCoroutine);
        }

        private IEnumerator Generate()
        {
            while (true)
            {
                yield return CheckAllRegions();
                yield return null;
            }
        }

        IEnumerator CheckAllRegions()
        {
            for (int i = 0; i < regionsOfGenerating.Length; i++)
            {
                if (slidingObjects[i]) continue;
                yield return GenerateSlidingObject(i);
            }
        }

        IEnumerator GenerateSlidingObject(int index)
        {
            yield return waitForGenerateDelay;
            slidingObjects[index] = regionsOfGenerating[index].Generate(slidingObjectPrefab);
            slidingObjects[index].Replace += Replace;
            slidingObjects[index].Replace += OnCageReplaced;
            yield return new WaitUntil(() => slidingObjects[index] != null);
        }

        private void OnCageReplaced(object sender, ReplaceObjectEventArgs e)
        {
            if(e is ReplaceCageEventArgs replaceCage)
            {
                if (replaceCage.isBroke)
                    CageBroke?.Invoke();
                else
                    CageOpened?.Invoke();
            }
        }
    }
}
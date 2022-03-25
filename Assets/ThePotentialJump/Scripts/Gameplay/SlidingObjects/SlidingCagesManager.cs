using System;
using System.Collections;
using ThePotentialJump.Sounds;
using UnityEngine;
using UnityEngine.Events;

namespace ThePotentialJump.Gameplay
{
    public class SlidingCagesManager : Replacable
    {
        [SerializeField] private SlidingObject[] slidingObjectPrefabs;
        [SerializeField] private RegionOfGenerating[] regionsOfGenerating;
        [SerializeField] private float generateDelay = 0.33f;
        [Space]
        [Header("SFX Modules")]
        [SerializeField] private SFXModule cageOpenedSFX;
        [SerializeField] private SFXModule cageBrokeSFX;
        [Space]
        [SerializeField] private ToleranceEnergyLabel MinEnergyLabel;
        [SerializeField] private ToleranceEnergyLabel MaxEnergyLabel;
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
            int rand = UnityEngine.Random.Range(0, slidingObjectPrefabs.Length);
            slidingObjects[index] = regionsOfGenerating[index].Generate(slidingObjectPrefabs[rand]);
            if(slidingObjects[index] is SlidingCage slidingCage)
            {
                MinEnergyLabel.SetTarget(slidingCage.transform, slidingCage.MinEnergyTolerance);
                MaxEnergyLabel.SetTarget(slidingCage.transform, slidingCage.MaxEnergyTolerance);
            }
            slidingObjects[index].Replace += Replace;
            slidingObjects[index].Replace += OnCageReplaced;
            //if(slidingObjects[index] is SlidingCage cage)
            //{
            //    //cage.CageBroke +=
            //    //cage.CageOpened +=
            //}
            yield return new WaitUntil(() => slidingObjects[index] != null);
        }

        private void OnCageReplaced(object sender, ReplaceObjectEventArgs e)
        {
            MinEnergyLabel.StopTarget();
            MaxEnergyLabel.StopTarget();
            if (e is ReplaceCageEventArgs replaceCage)
            {
                if (replaceCage.isBroke)
                {
                    CageBroke?.Invoke();
                    cageBrokeSFX.PlayImmediate();
                }
                else
                {
                    CageOpened?.Invoke();
                    cageOpenedSFX.PlayImmediate();
                }
            }
        }
    }
}
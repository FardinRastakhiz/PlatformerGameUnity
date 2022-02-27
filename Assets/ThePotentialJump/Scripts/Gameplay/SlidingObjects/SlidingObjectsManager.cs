using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ThePotentialJump.Gameplay
{

    public class SlidingObjectsManager : MonoBehaviour
    {
        [SerializeField] private SlidingObject slidingObjectPrefab;
        [SerializeField] private RegionOfGenerating[] regionsOfGenerating;
        [SerializeField] private UnityEvent CageDestroyed;
        private Coroutine generateCoroutine;
        private SlidingObject[] slidingObjects;
        private void Awake()
        {
            slidingObjects = new SlidingObject[regionsOfGenerating.Length];
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
                for (int i = 0; i < regionsOfGenerating.Length; i++)
                {
                    if (slidingObjects[i]) continue;
                    yield return GenerateSlidingObject(i);
                }
                yield break;
            }
        }

        IEnumerator GenerateSlidingObject(int index)
        {
            slidingObjects[index] = regionsOfGenerating[index].Generate(slidingObjectPrefab);
            slidingObjects[index].Destroyed += (o, e) => CageDestroyed?.Invoke();
            yield return new WaitUntil(() => slidingObjects[index] != null) ;
        }

    }

}
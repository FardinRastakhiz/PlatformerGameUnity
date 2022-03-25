using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThePotentialJump.Gameplay
{
    public class UpdateSkyWithHeight : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer blueSky;
        [SerializeField] private float baseHeight = 300;
        [SerializeField] private float fadeDistance = 200;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(UpdateSky());
        }

        IEnumerator UpdateSky()
        {
            while (true)
            {
                var height = transform.position.y;
                var fadeHeight = height - baseHeight;
                if (fadeHeight > 0 && fadeHeight < fadeDistance)
                {
                    var color = blueSky.color;
                    color.a = (fadeDistance - fadeHeight) / fadeDistance;
                    blueSky.color = color;
                }

                yield return null;
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThePotentialJump.Utilities
{
    public class SpritesGroup : MonoBehaviour
    {
        [SerializeField] private float alpha = 1.0f;

        private WaitForSeconds waitForFixedUpdate;
        private float epsilon = float.Epsilon;
        private float lastAlpha = 1.0f;
        public float Alpha { get => alpha; set => alpha = value > 0 ? (value < 1 ? value : 1) : 0; }
        private SpriteRenderer[] spriteRenderers;
        private void Start()
        {
            waitForFixedUpdate = new WaitForSeconds(Time.fixedDeltaTime);
            spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            StartCoroutine(UpdateChecking());
        }

        Coroutine fadeInOut;
        public void Show()
        {
            if (fadeInOut != null) StopCoroutine(fadeInOut);
            fadeInOut = StartCoroutine(FadeIn());
        }
        public void Hide()
        {
            if (fadeInOut != null) StopCoroutine(fadeInOut);
            fadeInOut = StartCoroutine(FadeOut());
        }


        IEnumerator FadeIn()
        {
            while (Alpha<1-float.Epsilon)
            {
                Alpha += Time.fixedDeltaTime * 2f;
                yield return waitForFixedUpdate;
            }
        }
        IEnumerator FadeOut()
        {
            while (Alpha > float.Epsilon)
            {
                Alpha -= Time.fixedDeltaTime * 2f;
                yield return waitForFixedUpdate;
            }
        }

        IEnumerator UpdateChecking()
        {
            while (true)
            {
                yield return waitForFixedUpdate;
                if (Mathf.Abs(Alpha - lastAlpha) > epsilon)
                {
                    for (int i = 0; i < spriteRenderers.Length; i++)
                    {
                        var col = spriteRenderers[i].color;
                        col.a = Alpha;
                        spriteRenderers[i].color = col;
                        lastAlpha = Alpha;
                    }
                }
            }
        }
    }
}

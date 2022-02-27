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
            var hashCode = this.gameObject.GetHashCode();
            StartCoroutine(UpdateChecking());
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

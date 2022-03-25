using System.Collections;
using UnityEngine;

namespace ThePotentialJump.Gameplay
{
    public class Molecule : MonoBehaviour
    {
        [SerializeField] private float increaseRate = 10;
        private static float rate = 0;
        private Rigidbody2D rigidbody;
        private SpriteRenderer spriteRenderer;
        public bool Changed { get; set; }
        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            rate = increaseRate;
        }

        private WaitForSeconds waitForSeconds;
        private void Start()
        {
            waitForSeconds = new WaitForSeconds(Time.deltaTime);
            StartCoroutine(UpdateForces());
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "White")
            {
                var otherMolecule = collision.gameObject.GetComponent<Molecule>();
                if (otherMolecule.Changed)
                {
                    // Destroy(this.gameObject);
                    otherMolecule.Changed = false;
                }
                else
                {
                    Changed = true;
                    gameObject.tag = "Untagged";
                    spriteRenderer.color = new Color(Color.black.r, Color.black.g, Color.black.b, spriteRenderer.color.a);
                    rate += 50;
                }
            }
        }

        IEnumerator UpdateForces()
        {
            while (true)
            {
                rigidbody.AddForce(RandomUnitVector() * rate * Time.deltaTime);
                yield return waitForSeconds;
            }
        }

        public Vector2 RandomUnitVector()
        {
            float random = Mathf.Deg2Rad * Random.Range(0f, 360f);
            return new Vector2(Mathf.Cos(random), Mathf.Sin(random));
        }
    }
}
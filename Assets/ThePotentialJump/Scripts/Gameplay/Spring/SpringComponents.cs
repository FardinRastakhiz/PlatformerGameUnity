using UnityEngine;
using System;

namespace ThePotentialJump.Gameplay
{
    [Serializable]
    public class SpringComponents
    {
        public Vector3 Position => spring.transform.position;
        public float MaxCompressCapacity => maxCompressCapacity;

        [Header("Spring parameters")]
        [SerializeField] private SpriteRenderer spring;
        private BoxCollider2D springCollider;
        [SerializeField]
        private float idleHeight = 7.0f;
        [SerializeField]
        private float maxCompressCapacity = 3.5f;


        public void SetupParameters(JumpRuler ruler)
        {
            springCollider = spring.GetComponent<BoxCollider2D>();
            SetSpringSize(0);
        }

        public void SetSpringSize(float compressAmount)
        {
            var size = idleHeight - compressAmount;
            spring.size = new Vector2(spring.size.x, size);
            springCollider.offset = new Vector2(0, size / 2.0f);
            springCollider.size = new Vector2(springCollider.size.x, size);
        }
    }
}
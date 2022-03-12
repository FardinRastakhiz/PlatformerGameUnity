using System;
using UnityEngine;

namespace ThePotentialJump.Gameplay
{
    [Serializable]
    public class ProjectileParameters
    {
        [SerializeField] private float weight = 50.0f;
        [SerializeField] private Sprite icon;

        public float Mass { get => weight; }
        public Sprite Icon { get => icon; }
    }
}
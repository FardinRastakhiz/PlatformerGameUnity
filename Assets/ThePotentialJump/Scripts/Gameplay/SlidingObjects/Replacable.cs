using System;
using UnityEngine;

namespace ThePotentialJump.Gameplay
{
    public abstract class Replacable : MonoBehaviour
    {
        public abstract event EventHandler<ReplaceObjectEventArgs> Replace;
        [SerializeField] private GameObject replaceObjectPrefab;

        public GameObject ReplaceObjectPrefab { get => replaceObjectPrefab; set => replaceObjectPrefab = value; }
    }
}
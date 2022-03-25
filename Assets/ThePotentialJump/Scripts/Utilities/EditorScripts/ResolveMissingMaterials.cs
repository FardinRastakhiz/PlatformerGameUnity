using UnityEngine;

namespace ThePotentialJump.EditorUtilities
{
    public class ResolveMissingMaterials : MonoBehaviour
    {
        [SerializeField] private Material replacingMaterial;
        public Material ReplacingMaterial => replacingMaterial;
    }
}
using UnityEngine;

public class ResolveMissingMaterials : MonoBehaviour
{
    [SerializeField] private Material replacingMaterial;
    public Material ReplacingMaterial => replacingMaterial;
}

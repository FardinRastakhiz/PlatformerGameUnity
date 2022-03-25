using System.Linq;
using ThePotentialJump.EditorUtilities;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ResolveMissingMaterials))]
public class ResolveMissingMaterialsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var myScript = (ResolveMissingMaterials)target;

        if (GUILayout.Button("Resolve Missing Materials"))
        {
            var sprites = FindObjectsOfType<SpriteRenderer>();
            var spritesWithMissedMaterials = sprites.Where(s => s.sharedMaterial == null);
            foreach (var spriteRenderer in spritesWithMissedMaterials)
            {
                spriteRenderer.sharedMaterial = myScript.ReplacingMaterial;
            } 
            Debug.Log("sprites count: " + sprites.Where(s => s.sharedMaterial == null).Count());
        }
    }
}
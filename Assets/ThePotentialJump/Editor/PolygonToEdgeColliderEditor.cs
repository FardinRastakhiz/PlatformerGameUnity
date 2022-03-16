using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace ThePotentialJump.EditorUtilities
{
    [CustomEditor(typeof(PolygonToEdgeCollider))]
    public class PolygonToEdgeColliderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Replace PolygonCollider with EdgeCollider"))
            {
                var myObject = (target as PolygonToEdgeCollider).gameObject;
                var polygonCollider = myObject.GetComponent<PolygonCollider2D>();
                if (polygonCollider == null) return;
                var points = polygonCollider.points;
                var edgeCollider = myObject.AddComponent<EdgeCollider2D>();
                edgeCollider.points = points;
                Destroy(polygonCollider);
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }

            if (GUILayout.Button("ShiftPointToRight"))
            {
                var myObject = (target as PolygonToEdgeCollider).gameObject;
                var edgeCollider = myObject.GetComponent<EdgeCollider2D>();
                if (edgeCollider == null) return;
                var points = edgeCollider.points;
                var lastPoint = points[points.Length - 1];
                for (int i = points.Length - 1; i > 0; i--)
                {
                    points[i] = points[i - 1];
                }
                points[0] = lastPoint;
                edgeCollider.points = points;
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }

    }
}
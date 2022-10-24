using UnityEngine;
using UnityEditor;

namespace Fallencake.Tools
{
	[CustomPropertyDrawer(typeof(FCReadOnlyAttribute))]
	public class FCReadOnlyAttributeDrawer : PropertyDrawer
	{
		// Determines the height of the GUI for this field is in pixels.
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
            return EditorGUI.GetPropertyHeight(property, label, true);
		}

        // Displays a field inside the inspector but doesn't allow for it to be edited
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
			GUI.enabled = false;
			EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
		}
	}
}
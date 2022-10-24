using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if TEXTMESHPRO_PRESENT
using TMPro;
#endif

// ReSharper disable InconsistentNaming

namespace ModelShark
{
    [CustomEditor(typeof (TooltipTrigger)), CanEditMultipleObjects]
    public class TooltipTriggerEditor : Editor
    {
        private SerializedProperty tooltipStyle;
        private SerializedProperty staysOpen;
        private SerializedProperty neverRotate;
        private SerializedProperty isBlocking;
        private static readonly string[] dontIncludeMe = { "m_Script" };

        private void OnEnable()
        {
            serializedObject.Update();

            tooltipStyle = serializedObject.FindProperty("tooltipStyle");
            staysOpen = serializedObject.FindProperty("staysOpen");
            neverRotate = serializedObject.FindProperty("neverRotate");
            isBlocking = serializedObject.FindProperty("isBlocking");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            TooltipTrigger tooltipTrigger = target as TooltipTrigger;

            if (tooltipTrigger != null)
            {
                // TOOLTIP STYLE
                GUI.color = tooltipStyle.objectReferenceValue == null ? Color.red : Color.white;
                EditorGUILayout.PropertyField(tooltipStyle, new GUIContent("Tooltip Style"));
                tooltipTrigger.tooltipStyle = tooltipStyle.objectReferenceValue as TooltipStyle;
                GUI.color = Color.white;

                if (tooltipTrigger.parameterizedTextFields == null)
                    tooltipTrigger.parameterizedTextFields = new List<ParameterizedTextField>();
                if (tooltipTrigger.dynamicImageFields == null)
                    tooltipTrigger.dynamicImageFields = new List<DynamicImageField>();
                if (tooltipTrigger.dynamicSectionFields == null)
                    tooltipTrigger.dynamicSectionFields = new List<DynamicSectionField>();
                
                if (tooltipTrigger.tooltipStyle != null)
                {
                    // Retrieve dynamic text and image fields on the tooltip.
                    List<string> textFieldsText = new List<string>();
#if TEXTMESHPRO_PRESENT
                    TextMeshProUGUI[] textFieldsTMP = tooltipTrigger.tooltipStyle.GetComponentsInChildren<TextMeshProUGUI>(true);
                    foreach (TextMeshProUGUI textFieldTMP in textFieldsTMP)
                        textFieldsText.Add(textFieldTMP.text);
#endif
                    Text[] textFields = tooltipTrigger.tooltipStyle.GetComponentsInChildren<Text>(true);
                    foreach (Text textField in textFields)
                        textFieldsText.Add(textField.text);

                    DynamicImage[] imageFields = tooltipTrigger.tooltipStyle.GetComponentsInChildren<DynamicImage>(true);
                    DynamicSection[] sectionFields = tooltipTrigger.tooltipStyle.GetComponentsInChildren<DynamicSection>(true);

                    // Fill and configure the dynamic text and image field collections on the tooltip trigger.
                    textFieldsText.FillParameterizedTextFields(ref tooltipTrigger.parameterizedTextFields, "%");

                    imageFields.FillDynamicImageFields(ref tooltipTrigger.dynamicImageFields, "%");
                    sectionFields.FillDynamicSectionFields(ref tooltipTrigger.dynamicSectionFields, "%");

                    // DYNAMIC TEXT FIELDS
                    if (tooltipTrigger.parameterizedTextFields.Count > 0)
                    {
                        EditorGUILayout.BeginVertical("Box");
                        EditorGUILayout.LabelField("Dynamic Text Fields", EditorStyles.boldLabel);
                        foreach (ParameterizedTextField field in tooltipTrigger.parameterizedTextFields)
                        {
                            EditorGUILayout.LabelField(field.name);
                            EditorStyles.textField.wordWrap = true;
                            field.value = EditorGUILayout.TextArea(field.value);
                        }
                        EditorGUILayout.Space();
                        EditorGUILayout.EndVertical();
                    }

                    // DYNAMIC IMAGE FIELDS
                    if (tooltipTrigger.dynamicImageFields.Count > 0)
                    {
                        EditorGUILayout.BeginVertical("Box");
                        EditorGUILayout.LabelField("Dynamic Image Fields", EditorStyles.boldLabel);
                        foreach (DynamicImageField field in tooltipTrigger.dynamicImageFields)
                        {
                            EditorGUILayout.LabelField(field.name);
                            field.replacementSprite =
                                EditorGUILayout.ObjectField(field.replacementSprite, typeof (Sprite), false) as Sprite;
                        }
                        EditorGUILayout.Space();
                        EditorGUILayout.EndVertical();
                    }

                    // DYNAMIC SECTIONS
                    if (tooltipTrigger.dynamicSectionFields.Count > 0)
                    {
                        EditorGUILayout.BeginVertical("Box");
                        EditorGUILayout.LabelField("Dynamic Sections", EditorStyles.boldLabel);
                        foreach (DynamicSectionField field in tooltipTrigger.dynamicSectionFields)
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField(field.name, GUILayout.Width(166));
                            field.isOn = EditorGUILayout.Toggle(field.isOn, GUILayout.Width(11));
                            EditorGUILayout.LabelField("Visible", GUILayout.Width(40));
                            EditorGUILayout.EndHorizontal();
                        }
                        EditorGUILayout.Space();
                        EditorGUILayout.EndVertical();
                    }
                }
                else
                {
                    // Clear the fields
                    tooltipTrigger.parameterizedTextFields.Clear();
                    tooltipTrigger.dynamicImageFields.Clear();
                    tooltipTrigger.dynamicSectionFields.Clear();
                }

                // STAYS OPEN?
                EditorGUILayout.PropertyField(staysOpen, new GUIContent("Stays Open?"));
                tooltipTrigger.staysOpen = staysOpen.boolValue;

                // IS BLOCKING?
                if (tooltipTrigger.staysOpen)
                {
                    EditorGUILayout.PropertyField(isBlocking, new GUIContent("    Is Blocking?"));
                    tooltipTrigger.isBlocking = isBlocking.boolValue;
                }
                else
                {
                    isBlocking.boolValue = false;
                    tooltipTrigger.isBlocking = false;
                }

                // NEVER ROTATE?
                EditorGUILayout.PropertyField(neverRotate, new GUIContent("Never Rotate"));
                tooltipTrigger.neverRotate = neverRotate.boolValue;
            }

            // Draw the rest of the fields using the default inspector.
            DrawPropertiesExcluding(serializedObject, dontIncludeMe);

            if (GUI.changed && !Application.isPlaying)
            {
                EditorUtility.SetDirty(tooltipTrigger);
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
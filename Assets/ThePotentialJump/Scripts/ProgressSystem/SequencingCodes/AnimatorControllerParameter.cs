using System;
using UnityEditor;
using UnityEngine;

namespace ThePotentialJump.ProgressSystem
{
    [Serializable]
    public class AnimatorControllerParameter
    {
        [SerializeField] private AnimatorControllerParameterType parameterType;
        [SerializeField] private string parameterName;
        [SerializeField] private bool boolValue;
        [SerializeField] private float floatValue;
        [SerializeField] private int intValue;

        public AnimatorControllerParameterType ParameterType { get => parameterType; set => parameterType = value; }
        public bool BoolValue { get => boolValue; set => boolValue = value; }
        public float FloatValue { get => floatValue; set => floatValue = value; }
        public int IntValue { get => intValue; set => intValue = value; }
        public string ParameterName { get => parameterName; set => parameterName = value; }

        internal void SetParameterFor(Animator animator)
        {
            switch (ParameterType)
            {
                case AnimatorControllerParameterType.Float:
                    animator.SetFloat(ParameterName, FloatValue);
                    break;
                case AnimatorControllerParameterType.Int:
                    animator.SetInteger(ParameterName, IntValue);
                    break;
                case AnimatorControllerParameterType.Bool:
                    animator.SetBool(ParameterName, BoolValue);
                    break;
                case AnimatorControllerParameterType.Trigger:
                    animator.SetTrigger(ParameterName);
                    break;
            }
        }
    }

}
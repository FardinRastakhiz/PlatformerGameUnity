using System;
using UnityEngine;

namespace ThePotentialJump.ProgressSystem
{
    internal class ConditionalFieldAttribute: Attribute
    {
        private string conditionField;
        private System.Enum conditionValue;

        public ConditionalFieldAttribute(string conditionField, System.Enum conditionValue)
        {
            this.conditionField = conditionField;
            this.conditionValue = conditionValue;
        }
    }
}
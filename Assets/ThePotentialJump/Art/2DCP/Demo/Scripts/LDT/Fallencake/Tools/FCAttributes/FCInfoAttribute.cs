using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Fallencake.Tools
{
	public class FCInfoAttribute : PropertyAttribute
	{
		public enum InformationType { Error, Info, None, Warning }

#if UNITY_EDITOR
		public string Message;
		public MessageType Type;
		public bool MessageAfterProperty;

		public FCInfoAttribute(string message, InformationType type, bool messageAfterProperty)
		{
			this.Message = message;
			if (type == InformationType.Error) { this.Type = UnityEditor.MessageType.Error; }
			if (type == InformationType.Info) { this.Type = UnityEditor.MessageType.Info; }
			if (type == InformationType.Warning) { this.Type = UnityEditor.MessageType.Warning; }
			if (type == InformationType.None) { this.Type = UnityEditor.MessageType.None; }
			this.MessageAfterProperty = messageAfterProperty;
		}
#else
		public FCInfoAttribute(string message, InformationType type, bool messageAfterProperty)
		{
			
		}
#endif
	}
}

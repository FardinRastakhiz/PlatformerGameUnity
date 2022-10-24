using UnityEngine;
using System.Collections;
using Fallencake.Tools;
using UnityEditor;

namespace Fallencake.Tools
{
	/// <summary>
	/// This class allows to enable and to disable the help information from the inspectors using Unity's top menu
	/// </summary>
	public static class FCHelpInfoMenu
	{
		[MenuItem("Tools/Fallencake/Preferences/Enable Help in Inspectors", false, 0)]
		/// <summary>
		/// Adds a menu item to enable help
		/// </summary>
		private static void EnableHelpInInspectors()
		{
			SetHelpEnabled(true);
		}

		[MenuItem("Tools/Fallencake/Preferences/Enable Help in Inspectors", true)]
		/// <summary>
		/// Determines if the "Enable Help" entry should be greyed or not
		/// </summary>
		private static bool EnableHelpInInspectorsValidation()
		{
			return !HelpEnabled();
		}

		[MenuItem("Tools/Fallencake/Preferences/Disable Help in Inspectors", false, 1)]
		/// <summary>
		/// Adds a menu item to disable help
		/// </summary>
		private static void DisableHelpInInspectors()
		{
			SetHelpEnabled(false);
		}

		[MenuItem("Tools/Fallencake/Preferences/Disable Help in Inspectors", true)]
		/// <summary>
		/// Determines if the "Disable Help" entry should be greyed or not
		/// </summary>
		private static bool DisableHelpInInspectorsValidation()
		{
			return HelpEnabled();
		}

		/// <summary>
		/// Checks editor prefs to see if help is enabled or not
		/// </summary>
		/// <returns><c>true</c>, if enabled was helped, <c>false</c> otherwise.</returns>
		private static bool HelpEnabled()
		{
			if (EditorPrefs.HasKey("FCShowHelpInInspectors"))
			{
				return EditorPrefs.GetBool("FCShowHelpInInspectors");
			}
			else
			{
				EditorPrefs.SetBool("FCShowHelpInInspectors", true);
				return true;
			}
		}

		/// <summary>
		/// Sets the help enabled editor pref.
		/// </summary>
		/// <param name="status">If set to <c>true</c> status.</param>
		private static void SetHelpEnabled(bool status)
		{
			EditorPrefs.SetBool("FCShowHelpInInspectors", status);
			SceneView.RepaintAll();

		}
	}
}
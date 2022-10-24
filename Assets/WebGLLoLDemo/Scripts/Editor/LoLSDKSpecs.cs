using System.IO;
using SimpleJSON;
using UnityEditor;
using UnityEditor.Build;
#if UNITY_2018_1_OR_NEWER
using UnityEditor.Build.Reporting;
#endif
using UnityEngine;

namespace LoL.Editor
{
    public class LoLSDKSpecs :
#if UNITY_2018_1_OR_NEWER
        IPostprocessBuildWithReport
#else
        IPostprocessBuild
#endif
    {
        public int callbackOrder => 0;
        const string lolSpecFilename = "lol_spec.json";
        const string lol_sdk_version = "lol_sdk_version";
        const string manuallyAddVersionMsg = "MANUALLY ADD LOL SDK VERSION";

        [UnityEditor.Callbacks.DidReloadScripts]
        static void GenerateSDKSpec ()
        {
            var editorPath = Application.streamingAssetsPath + "/" + lolSpecFilename;

            if (File.Exists(editorPath))
                return;

            var sdkVersionFieldInfo = typeof(LoLSDK.WebGL).GetField("SDK_VERSION");
            var lolSdkVersion = sdkVersionFieldInfo == null
                ? manuallyAddVersionMsg
                : (string) sdkVersionFieldInfo.GetValue(null);

            var spec = new JSONObject
            {
                ["unity_version"] = Application.unityVersion,
                [lol_sdk_version] = lolSdkVersion
            };

            File.WriteAllText(editorPath, spec.ToString(4));
            AssetDatabase.Refresh();

            Debug.Log(string.Format("{0} file is generated at {1}",
                lolSpecFilename,
                editorPath));

            if (lolSdkVersion == manuallyAddVersionMsg)
            {
                Debug.LogError(string.Format("Please replace \"{0}\" with the version of the LoLSDK you are using in the {1} file.\nex: \"{2}\": \"4\"",
                    manuallyAddVersionMsg,
                    lolSpecFilename,
                    lol_sdk_version));
            }
        }

#if UNITY_2018_1_OR_NEWER
        public void OnPostprocessBuild (BuildReport report)
        {
            if (report.summary.platform != BuildTarget.WebGL)
                return;

            AddSpecToBuild(report.summary.outputPath);
        }
#else
        public void OnPostprocessBuild (BuildTarget target, string path)
        {
            if (target != BuildTarget.WebGL)
                return;

            AddSpecToBuild(path);
        }
#endif

        void AddSpecToBuild (string buildPath)
        {
            var editorPath = Application.streamingAssetsPath + "/" + lolSpecFilename;
            // Copy sdk spec to build root folder.
            var spec = JSON.Parse(File.ReadAllText(editorPath));

            if (spec[lol_sdk_version] == manuallyAddVersionMsg)
            {
                Debug.LogError(string.Format("You MUST manually updated the {0} \"{1}\" value to your current SDK version.\nex: \"{1}\": \"4\"",
                    lolSpecFilename,
                    lol_sdk_version));
            }

            var lolSdkVersion = (string) typeof(LoLSDK.WebGL).GetField("SDK_VERSION")?.GetValue(null);
            var upgradedEditorOrSDK = false;
            if (spec["unity_version"] != Application.unityVersion)
            {
                spec["unity_version"] = Application.unityVersion;
                upgradedEditorOrSDK = true;
            }

            if (lolSdkVersion != null && spec[lol_sdk_version] != lolSdkVersion)
            {
                spec[lol_sdk_version] = lolSdkVersion;
                upgradedEditorOrSDK = true;
            }

            if (upgradedEditorOrSDK)
            {
                Debug.Log(string.Format("{0} was updated!", lolSpecFilename));
                File.WriteAllText(editorPath, spec.ToString(4));
                AssetDatabase.Refresh();
            }

            // Remove streaming assets lol spec from build.
            File.Delete(buildPath + "/StreamingAssets/" + lolSpecFilename);

            spec.Add("build_dir_size", GetDirectorySize(buildPath + "/Build"));
            spec.Add("streaming_assets_dir_size", GetDirectorySize(buildPath + "/StreamingAssets"));

            File.WriteAllText(buildPath + "/" + lolSpecFilename, spec.ToString(4));
        }

        long GetDirectorySize (string directoryPath)
        {
            var files = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories);

            long length = 0;
            foreach (string name in files)
            {
                FileInfo info = new FileInfo(name);
                length += info.Length;
            }
            return length;
        }
    }
}
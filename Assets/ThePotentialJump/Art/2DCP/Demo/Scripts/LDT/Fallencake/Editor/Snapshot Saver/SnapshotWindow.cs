using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_WINRT
using UnityEngine.Windows;
using File = UnityEngine.Windows.File;
using Directory = UnityEngine.Windows.Directory;
 
#else
using System.IO;
using File = System.IO.File;
using Directory = System.IO.Directory;
using DirectoryInfo = System.IO.DirectoryInfo;
#endif

#if UNITY_EDITOR
[System.Serializable] public class SnapshotWindow : EditorWindow
{
    [SerializeField] public RenderTexture sourceImg;
    [SerializeField] public bool showRes = true;
    [Range(128, 4096)] public int ImgResX = 1024;
    [Range(128, 4096)] public int ImgResY = 1024;
    [SerializeField] public string nameTag = "Snapshot 00";
    [SerializeField] public string saveDirectory = "/2DCP/Sprites/Snapshots/";
    //public Material litMaterial;
    [SerializeField] public Material unlitMaterial;
    [SerializeField] public GameObject Target;
    [SerializeField] public Camera Camera;
    private SpriteRenderer[] sprites;
    protected Vector3 camPos = new Vector3(0, 500, -50);

    private GUIStyle camButtonStyle;
    private GUIStyle snapButtonStyle;

    [MenuItem("Tools/Fallencake/Snapshot Saver")]
    public static void ShowWindow ()
    {
        EditorWindow.GetWindow<SnapshotWindow>("Snapshot Saver");
    }

    protected void OnEnable()
    {
        unlitMaterial = new Material(Shader.Find("Sprites/Default"));
        Camera [] Cameras = FindObjectsOfType<Camera>();
        if (Cameras.Length != 0)
        {
            foreach (Camera i in Cameras)
            {
                if (i.name.Contains("[Snapshot Camera]"))
                {
                    Camera = i;
                    SetCam();
                    break;
                }
            }
        }
    }

    public void OnGUI()
	{
        //unlitMaterial = new Material(Shader.Find("Sprites/Default"));

        camButtonStyle = new GUIStyle(GUI.skin.button)
        {
            alignment = TextAnchor.MiddleCenter,
            margin = new RectOffset(),
            padding = new RectOffset(),
            fontSize = 15,
            fontStyle = FontStyle.Bold
        };

        snapButtonStyle = new GUIStyle(GUI.skin.button)
        {
            alignment = TextAnchor.MiddleLeft,
            margin = new RectOffset(0,0,10,10),
            padding = new RectOffset(10,10,5,5),
            fontStyle = FontStyle.Bold
        };

        if (Camera == null)
        {

            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
        
            if (GUILayout.Button(new GUIContent("Create Camera", EditorGUIUtility.ObjectContent(null, typeof(Camera)).image), camButtonStyle, GUILayout.Height(100), GUILayout.Width(200)))
            {
                Undo.RecordObject(this, "Set Camera");
                SetCam();
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.EndArea();
        }
        else
        {
            Undo.RecordObject(this, "Draw GUI Layout");
            //sourceImg = (RenderTexture)EditorGUILayout.ObjectField("Render Texture:", sourceImg, typeof(RenderTexture), true);

            showRes = EditorGUILayout.BeginFoldoutHeaderGroup(showRes, "Resolution", null, ShowHeaderContextMenu);
            if (showRes)
            {
                ImgResX = EditorGUILayout.IntSlider("X:", ImgResX, 128, 4096);
                ImgResY = EditorGUILayout.IntSlider("Y:", ImgResY, 128, 4096);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            Camera = EditorGUILayout.ObjectField("Camera:", Camera, typeof(Camera), true) as Camera;
            nameTag = EditorGUILayout.TextField("Name Tag:", nameTag);
            saveDirectory = EditorGUILayout.TextField("Save Directory:", saveDirectory);
            //litMaterial = (Material)EditorGUILayout.ObjectField("Lit Material:", litMaterial, typeof(Material), true);
            unlitMaterial = (Material)EditorGUILayout.ObjectField("Unlit Material:", unlitMaterial, typeof(Material), true);
            Target = EditorGUILayout.ObjectField("Target:", Target, typeof(GameObject), true) as GameObject;

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button(new GUIContent("           Set Target", EditorGUIUtility.IconContent("d_Selectable Icon").image), snapButtonStyle, GUILayout.Width(200)))
            {
                Undo.RecordObject(this, "Set Target");
                SetTarget();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button(new GUIContent("       Save Snapshot", EditorGUIUtility.IconContent("d_FrameCapture@2x").image), snapButtonStyle, GUILayout.Width(200)))
            {
                if (canSave())
                {
                    Undo.RecordObject(this, "Save Snapshot");
                    SaveImage();
                }
                else
                {
                    Debug.Log("Set a Target object First!");
                }
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }

    void ShowHeaderContextMenu(Rect position)
    {
        var menu = new GenericMenu();
        menu.AddItem(new GUIContent("Set Default"), false, OnItemClicked);
        menu.DropDown(position);
    }

    void OnItemClicked()
    {
        Undo.RecordObject(this, "Set image resolution value to Default");
        ImgResX = 1024;
        ImgResY = 1024;
    }

    void OnInspectorUpdate()
    {
        /*if (Camera != null)
            Camera.transform.position = camPos;*/

        if (sourceImg != null)
        {
            if (sourceImg.width != ImgResX || sourceImg.height != ImgResY)
            {
                sourceImg = new RenderTexture(ImgResX, ImgResY, 24, RenderTextureFormat.ARGB32);
                Camera.targetTexture = sourceImg;
            }
        }
    }



    void SetCam()
    {
        Undo.RecordObject(this, "Set the Camera");
        if (Camera == null)
        {
            Camera = new GameObject("[Snapshot Camera]", typeof(Camera)).GetComponent<Camera>();
            Vector3 pos = Camera.transform.position;
            Camera.transform.position = new Vector3(pos.x, camPos.y, camPos.z);
        }
        else
        {
            //Debug.Log("The Snapshot Camera has been already set!");
            if(!Camera.name.Contains("[Snapshot Camera]"))
            {
                Camera.name = "[Snapshot Camera] " + Camera.name;
            }
            Vector3 pos = Camera.transform.position;
            Camera.transform.position = new Vector3(pos.x, pos.y, camPos.z);
        }
        sourceImg = new RenderTexture(ImgResX, ImgResY, 24, RenderTextureFormat.ARGB32);
        Camera.targetTexture = sourceImg;
        Camera.cullingMask = 1 << 0;
    }


    void SetTarget()
	{
        if (Target == null)
        {
            var selectedTarget = Selection.activeGameObject;
            Target = Instantiate(selectedTarget);
        }
        Vector3 newPosition = new Vector3(Camera.transform.position.x, Camera.transform.position.y, 0);
        Target.transform.position = newPosition;
        sprites = Target.GetComponentsInChildren<SpriteRenderer>();
        if (sprites != null)
        {
            foreach (SpriteRenderer sprite in sprites)
            {
                sprite.sharedMaterial = unlitMaterial;
            }
        }
        Selection.activeGameObject = Target;
        SceneView.FrameLastActiveSceneView();
        Selection.activeGameObject = Camera.gameObject;
    }

    void SaveImage()
    {
        var img = toTexture2D(sourceImg);
        var bytes = img.EncodeToPNG();
        var dirCount = DirCount(new DirectoryInfo(Application.dataPath + saveDirectory));
        while (File.Exists(Application.dataPath + saveDirectory + nameTag + dirCount.ToString() + ".png"))
        {
            dirCount++;
        }
        var path = Application.dataPath + saveDirectory + nameTag + dirCount.ToString() + ".png";
        File.WriteAllBytes(path, bytes);
        /*
        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.sharedMaterial = litMaterial;
        }
        canSave = false;*/
        AssetDatabase.Refresh();
    }

    public bool canSave()
    {
        bool canSave = Target != null;
        return canSave;
    }

    public Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGBA32, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }

    public static long DirCount(DirectoryInfo d)
    {
        long i = 0;
        FileInfo[] fis = d.GetFiles();
        foreach (FileInfo fi in fis)
        {
            if (fi.Extension.Equals(".png", StringComparison.OrdinalIgnoreCase))
                i++;
        }
        return i;
    }
}
#endif

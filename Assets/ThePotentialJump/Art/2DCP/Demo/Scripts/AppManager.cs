using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour
{
    public bool updateFrameRate = false;
    public int frameRate = 60;

    public bool optimizedReflections = false;

    public Camera waterCamera;

    [SerializeField]
    Camera[] waterCameras;

    [SerializeField]
    RenderTexture[] waterTextures;

    void Start()
    {
        SetFrameRate(frameRate);

        if (optimizedReflections)
        {
            StartCoroutine("GetReflection");
        }
    }

    void SetFrameRate(int frameRate)
    {
        if (Application.targetFrameRate != frameRate)
        {
            Application.targetFrameRate = frameRate;
        }
    }

    private IEnumerator GetReflection()
    {
        //deactivating all additional water cameras
        for (int i = 0; i < waterCameras.Length; i++)
        {
            waterCameras[i].gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(0.01f);

        //capturing images for all render textures
        for (int i = 0; i < waterTextures.Length; i++)
        {
            waterCamera.transform.position = waterCameras[i].transform.position;
            waterCamera.targetTexture = waterTextures[i];
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.01f);

        //deactivating main water camera
        waterCamera.gameObject.SetActive(false);
    }


    void Update()
    {
        if (updateFrameRate)
        {
            SetFrameRate(frameRate);
        }
        else if (Application.targetFrameRate != -1)
        {
            SetFrameRate(-1);
        }

       if (Input.GetKeyDown("r"))
       {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name, LoadSceneMode.Single);
       }
    }
}

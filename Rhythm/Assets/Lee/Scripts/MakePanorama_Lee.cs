using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakePanorama_Lee : MonoBehaviour
{
    public Camera MainCamera;
    public RenderTexture cubeR;
    public RenderTexture cubeL;
    public RenderTexture equireCube;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Capture();
    }

    private void Capture()
    {
        Debug.Log("start");
        MainCamera.stereoSeparation = 0.065f;
        MainCamera.RenderToCubemap(cubeL, 63, Camera.MonoOrStereoscopicEye.Left);
        MainCamera.RenderToCubemap(cubeR, 63, Camera.MonoOrStereoscopicEye.Right);
        cubeL.ConvertToEquirect(equireCube, Camera.MonoOrStereoscopicEye.Left);
        cubeR.ConvertToEquirect(equireCube, Camera.MonoOrStereoscopicEye.Right);

        Texture2D tex = new Texture2D(equireCube.width, equireCube.height);
        RenderTexture.active = equireCube;
        tex.ReadPixels(new Rect(0, 0, equireCube.width, equireCube.height), 0, 0);
        RenderTexture.active = null;
        byte[] bytes = tex.EncodeToJPG();
        string path = Application.dataPath + "/StreoPanorama.jpg";
        System.IO.File.WriteAllBytes(path, bytes);

        

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager_Lee : MonoBehaviour
{
    public void GoNextScene()
    {
        SceneManager.LoadScene("Tuto");
    }
}

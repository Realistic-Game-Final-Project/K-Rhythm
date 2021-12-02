using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerPjw : MonoBehaviour
{
    //call -> change scene
    private void MoveNextScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OpeningManager : MonoBehaviour
{
    [SerializeField]
    private Animator Dissolve;
    [SerializeField]
    private GameObject YonggoChae;

    public IEnumerator DissolveScene()
    {
        Dissolve.SetTrigger("Black");
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("SelectInstrumentScene");
    }


}

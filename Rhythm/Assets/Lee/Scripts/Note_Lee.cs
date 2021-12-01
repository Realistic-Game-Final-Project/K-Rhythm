using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note_Lee : MonoBehaviour
{
    public float NSpeed;

    UnityEngine.UI.Image noteImage;
    // Start is called before the first frame update
    void Start()
    {
        noteImage = GetComponent<UnityEngine.UI.Image>();

    }

    // Update is called once per frame
    void Update()
    {
        //Note Move
        transform.localPosition += Vector3.right * NSpeed * Time.deltaTime;
    }

    public void Hide()
    {
        //Note Image Hide
        noteImage.enabled = false;
    }
}

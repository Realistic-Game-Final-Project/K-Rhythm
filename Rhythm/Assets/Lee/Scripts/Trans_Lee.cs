using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trans_Lee : MonoBehaviour
{
    public Transform Mom;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Mom.localPosition;
    }
}

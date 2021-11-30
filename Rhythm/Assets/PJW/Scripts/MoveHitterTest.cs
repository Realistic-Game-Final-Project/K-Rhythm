using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHitterTest : MonoBehaviour
{
    private Vector3 left_v  = new Vector3(-3f, 0, 0);
    private Vector3 right_v = new Vector3(3f, 0, 0);
    private Vector3 up_v = new Vector3(0, 0, 1f);
    private Vector3 down_v = new Vector3(0, 0, -1f);
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += left_v;            
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += right_v;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.position += up_v;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.position += down_v;
        }
        //Debug.Log(transform.position);
    }
}

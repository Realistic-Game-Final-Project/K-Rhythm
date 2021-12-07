using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRMove_Lee : MonoBehaviour
{

    public GameObject Cam;
    public GameObject VRCam;
    public bool useVR;

    private void Start()
    {
        if (useVR)
        {
            VRCam.SetActive(true);
        }
        else
            Cam.SetActive(true);
    }

    private void Update()
    {
        PlayerMove();
        PlayerRotation();
    }
    #region 이동
    public float speed = 3;
    void PlayerMove()
    {
        Vector3 dirH;
        Vector3 dirV;
        
        if (useVR)
        {
            Vector2 p = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick, OVRInput.Controller.LTouch);
            if (p.x == 0 && p.y == 0)
                return;

            dirH = Camera.main.transform.right * p.x;
            dirV = Camera.main.transform.forward * p.y;
            Vector3 dir = dirH + dirV;
            dir.y = 0;
            dir.Normalize();

            transform.position += dir * speed * Time.deltaTime;
        }
        else
        {
            if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0) return;

            dirH = transform.right * Input.GetAxisRaw("Horizontal");
            dirV = transform.forward * Input.GetAxisRaw("Vertical");
            Vector3 dir = dirH + dirV;
            dir.y = 0;
            dir.Normalize();

            transform.position += dir * speed * Time.deltaTime;
        }

    }
    #endregion
    #region 회전
    public float RotSpeed = 40f;
    float y;
    float rotX;
    float rotY;
    void PlayerRotation()
    {
        if(useVR)
        {
            Vector2 thumb = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick, OVRInput.Controller.RTouch);
            float v = thumb.x;

            y += v * RotSpeed * Time.deltaTime;

            transform.localEulerAngles = new Vector3(0, y, 0);
        }
        else
        {
            float mx = Input.GetAxis("Mouse X");
            float my = Input.GetAxis("Mouse Y");

            rotX += my * RotSpeed * Time.deltaTime;
            rotY += mx * RotSpeed * Time.deltaTime;
            rotX = Mathf.Clamp(rotX, -60, 60);

            transform.localEulerAngles = new Vector3(0, rotY, 0);
            Cam.transform.localEulerAngles = new Vector3(-rotX, 0, 0);
        }
        

    }
    #endregion
}

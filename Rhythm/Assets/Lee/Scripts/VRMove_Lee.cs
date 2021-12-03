using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRMove_Lee : MonoBehaviour
{
    private void Update()
    {
        PlayerMove();
        PlayerRotation();
    }

    void PlayerMove()
    {
        Vector3 dirH;
        Vector3 dirV;
        float speed = 3;

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
    #region È¸Àü
    float RotSpeed = 40f;
    float y;
    float rotX;
    float rotY;
    void PlayerRotation()
    {
        Vector2 thumb = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick, OVRInput.Controller.RTouch);
        float v = thumb.x;

        y += v * RotSpeed * Time.deltaTime;

        transform.localEulerAngles = new Vector3(0, y, 0);

    }
    #endregion
}

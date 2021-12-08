using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteMove_Banghyang_Pjw : MonoBehaviour
{
    [SerializeField] private int note_speed;
    void Update()
    {
        transform.localPosition += Vector3.down * note_speed * Time.deltaTime;
    }
}

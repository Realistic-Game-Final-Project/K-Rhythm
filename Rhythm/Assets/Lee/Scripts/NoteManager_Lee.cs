using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager_Lee : MonoBehaviour
{
    MyTextDataArray_Lee GetBeat;
    public GameObject Musician; 
    double currentTime = 0;
    [SerializeField] Transform Nappear = null;
    [SerializeField] GameObject GNote = null;
    public Transform NEnd;

    TimingManager_Lee TimingManager;
    private void Start()
    {
        TimingManager = GetComponent<TimingManager_Lee>();
        GetBeat = Musician.GetComponent<Music>().mytext;
        StartCoroutine("NoteGame");
        Debug.Log("start");
        
    }
    private void Update()
    {
       
    }

    IEnumerator NoteGame()
    {
        //Note Instant
        for(int i =0; i<GetBeat.music.Length; i++)
        {
            GameObject NowNote = Instantiate(GNote, Nappear.position, Quaternion.identity);
            NowNote.transform.SetParent(this.transform);
            TimingManager.boxNoteList.Add(NowNote);
            yield return new WaitForSeconds(GetBeat.music[i].beat);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Note Destroy
        if(collision.CompareTag("Note"))
        {
            TimingManager.boxNoteList.Remove(collision.gameObject);
            Destroy(collision.gameObject);
        }
    }
}

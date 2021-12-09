using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager_Lee : MonoBehaviour
{
    MyTextDataArray GetBeat;
    public GameObject Musician; 
    double currentTime = 0;
    [SerializeField] Transform Nappear = null;
    [SerializeField] GameObject[] GNote = null;
    public Transform NEnd;


    TimingManager_Lee TimingManager;

    private void Start()
    {
        TimingManager = GetComponent<TimingManager_Lee>();
        GetBeat = BackgroundMusic.Instance.mytext;
        StartCoroutine("NoteGame");
        Debug.Log("start");

    }
   

    IEnumerator NoteGame()
    {
        //Note Instant
        for(int i =0; i<GetBeat.music.Length; i++)
        {
            if(GetBeat.music[i].beat >=1)
            {
                GameObject NowNote = Instantiate(GNote[0], Nappear.position, Quaternion.identity);
                NowNote.transform.SetParent(this.transform);
                TimingManager.boxNoteList.Add(NowNote);
            }
            else if(GetBeat.music[i].beat == 0.5f)
            {
                GameObject NowNote = Instantiate(GNote[1], Nappear.position, Quaternion.identity);
                NowNote.transform.SetParent(this.transform);
                TimingManager.boxNoteList.Add(NowNote);
            }
            else if (GetBeat.music[i].beat == 0.25f)
            {
                GameObject NowNote = Instantiate(GNote[2], Nappear.position, Quaternion.identity);
                NowNote.transform.SetParent(this.transform);
                TimingManager.boxNoteList.Add(NowNote);
            }


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

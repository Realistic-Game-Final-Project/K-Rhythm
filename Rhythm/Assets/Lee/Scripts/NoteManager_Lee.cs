using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class NoteManager_Lee : MonoBehaviour
{
    private static NoteManager_Lee instance;
    public static NoteManager_Lee Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<NoteManager_Lee>();
            }
            return instance;
        }
    }

    //public GameObject Musician; 
    double currentTime = 0;
    public Transform NEnd;
    
    [SerializeField] Transform Nappear = null;
    [SerializeField] GameObject[] GNote = null;    
    TimingManager_Lee TimingManager;

    //PJW : 전체 자료구조를 복사할 변수
    public List<Tuple<int, float>> selected_list = new List<Tuple<int, float>>();
    public int selected_music_number;

    /* <12/11 해결>
     * Awake()의 StartCoroutine이 문제였음.
     * 해당 스크립트가 생성된 순간 코루틴이 재생되는데 , 데이터를 읽는건 그 이후임
     * 즉, 데이터는 이상없이 저장되고 있었으나 코루틴의 재생 시간이 문제
     * 기존 코드는 거의 수정한게 없음. 새로운 함수를 추가했을 뿐.
     */

    private void Awake()
    {
        TimingManager = GetComponent<TimingManager_Lee>();     
    }

    public void CheckLoadDataSuccess() //Pjw : 현재 선택된 음악에 따라 로컬 자료구조에 static 알맞은 자료구조를 복사  
    {
        if (StaticDataPjw.is_inuyasha_selected == true)
        {
            selected_list = MusicDataPjw.music_inuyasha;
        }
        else if (StaticDataPjw.is_letitgo_selected == true)
        {
            selected_list = MusicDataPjw.music_letitgo;
        }
        else if (StaticDataPjw.is_cannon_selected == true)
        { 
            selected_list = MusicDataPjw.music_cannon;
        }
    }

    public void StartNoteGameCoroutine()
    {
        StartCoroutine("NoteGame");
    }    

    IEnumerator NoteGame()
    {
        float current_beat = -1;        
        //Note Instant
        for (int i =0; i< selected_list.Count; i++) 
        {
            current_beat =selected_list[i].Item2; //Pjw : 이제 여기서 선언한 자료구조에서 비트 데이터를 읽어 옵니다.            
            if (current_beat >= 1)
            {
                GameObject NowNote = Instantiate(GNote[0], Nappear.position, Quaternion.identity);
                NowNote.transform.SetParent(this.transform);
                TimingManager.boxNoteList.Add(NowNote);
            }
            else if(current_beat == 0.5f)
            {
                GameObject NowNote = Instantiate(GNote[1], Nappear.position, Quaternion.identity);
                NowNote.transform.SetParent(this.transform);
                TimingManager.boxNoteList.Add(NowNote);
            }
            else if (current_beat == 0.25f)
            {
                GameObject NowNote = Instantiate(GNote[2], Nappear.position, Quaternion.identity);
                NowNote.transform.SetParent(this.transform);
                TimingManager.boxNoteList.Add(NowNote);
            }
            yield return new WaitForSeconds(current_beat);
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

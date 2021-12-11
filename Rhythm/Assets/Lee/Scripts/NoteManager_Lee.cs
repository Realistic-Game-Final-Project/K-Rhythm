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

    //PJW : ��ü �ڷᱸ���� ������ ����
    public List<Tuple<int, float>> selected_list = new List<Tuple<int, float>>();
    public int selected_music_number;

    /* <12/11 �ذ�>
     * Awake()�� StartCoroutine�� ��������.
     * �ش� ��ũ��Ʈ�� ������ ���� �ڷ�ƾ�� ����Ǵµ� , �����͸� �д°� �� ������
     * ��, �����ʹ� �̻���� ����ǰ� �־����� �ڷ�ƾ�� ��� �ð��� ����
     * ���� �ڵ�� ���� �����Ѱ� ����. ���ο� �Լ��� �߰����� ��.
     */

    private void Awake()
    {
        TimingManager = GetComponent<TimingManager_Lee>();     
    }

    public void CheckLoadDataSuccess() //Pjw : ���� ���õ� ���ǿ� ���� ���� �ڷᱸ���� static �˸��� �ڷᱸ���� ����  
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
            current_beat =selected_list[i].Item2; //Pjw : ���� ���⼭ ������ �ڷᱸ������ ��Ʈ �����͸� �о� �ɴϴ�.            
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

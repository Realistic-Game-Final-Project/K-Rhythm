using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//12/13 
//이 코드를 테스트 해본 결과
//여기서 문제가 일어나는 경우는 collision이 감지가 안되서
//데이터가 사라지지 않는 경우 일 뿐
//각각의 자료구조는 올바르게 동작.
public class CollisionAndUpdatingQueuePjw : MonoBehaviour
{
    private Dictionary<string, int> gayageum_scale_tag_between_queue_index = new Dictionary<string, int>();
    private Dictionary<string, int> banghyang_scale_tag_between_queue_index = new Dictionary<string, int>();
    private void Awake()
    {
        gayageum_scale_tag_between_queue_index.Add("end_1", (int)GAYAGEUM_SCALE_NUMBER.TWO);
        gayageum_scale_tag_between_queue_index.Add("end_2", (int)GAYAGEUM_SCALE_NUMBER.THREE);
        gayageum_scale_tag_between_queue_index.Add("end_3", (int)GAYAGEUM_SCALE_NUMBER.FOUR);
        gayageum_scale_tag_between_queue_index.Add("end_4", (int)GAYAGEUM_SCALE_NUMBER.FIVE);
        gayageum_scale_tag_between_queue_index.Add("end_5", (int)GAYAGEUM_SCALE_NUMBER.SIX);
        gayageum_scale_tag_between_queue_index.Add("end_6", (int)GAYAGEUM_SCALE_NUMBER.SEVEN);
        gayageum_scale_tag_between_queue_index.Add("end_7", (int)GAYAGEUM_SCALE_NUMBER.EIGHT);
        gayageum_scale_tag_between_queue_index.Add("end_8", (int)GAYAGEUM_SCALE_NUMBER.NINE);
        gayageum_scale_tag_between_queue_index.Add("end_9", (int)GAYAGEUM_SCALE_NUMBER.TEN);
        gayageum_scale_tag_between_queue_index.Add("end_10", (int)GAYAGEUM_SCALE_NUMBER.ELEVEN);
        gayageum_scale_tag_between_queue_index.Add("end_11", (int)GAYAGEUM_SCALE_NUMBER.TWELVE);

        banghyang_scale_tag_between_queue_index.Add("end_0", (int)BANGHYANG_SCALE_NUMBER.ONE);
        banghyang_scale_tag_between_queue_index.Add("end_2", (int)BANGHYANG_SCALE_NUMBER.THREE);
        banghyang_scale_tag_between_queue_index.Add("end_4", (int)BANGHYANG_SCALE_NUMBER.FIVE);
        banghyang_scale_tag_between_queue_index.Add("end_5", (int)BANGHYANG_SCALE_NUMBER.SIX);
        banghyang_scale_tag_between_queue_index.Add("end_7", (int)BANGHYANG_SCALE_NUMBER.EIGHT);
        banghyang_scale_tag_between_queue_index.Add("end_9", (int)BANGHYANG_SCALE_NUMBER.TEN);
        banghyang_scale_tag_between_queue_index.Add("end_11", (int)BANGHYANG_SCALE_NUMBER.TWELVE);
        banghyang_scale_tag_between_queue_index.Add("end_12", (int)BANGHYANG_SCALE_NUMBER.THIRTEEN);
        banghyang_scale_tag_between_queue_index.Add("end_14", (int)BANGHYANG_SCALE_NUMBER.FIFTEEN);        
    }

    //TODO : 지금 사용자가 누르지 않았을 때 collision 부분과 겹쳐져서 , 제거 되는 코드가 에러가 있어서
    //일단 collision 검사 부분을 뒤로 밀어 버리는 방식을 썼으나 이는 임시 방편일 뿐 고쳐야 함.
    //SOL : 그냥 여기 지나면 삭제
    private void OnTriggerExit2D(Collider2D collision)
    {
        int index = gayageum_scale_tag_between_queue_index[tag];
        //Debug.Log("자료구조" + gameObject.tag +"  " + index + "에서 제거");
        PopFromSelectedQueue(gameObject.tag);
        Destroy(collision.gameObject);
    }

    private void PopFromSelectedQueue(string tag)
    {
        int index = 0;
        if (StaticDataPjw.is_gayageum_selected == true)
        {
            index = gayageum_scale_tag_between_queue_index[tag];           
            RhythmGameOnSelectedSheetPjw.Instance.unity_editor_current_scales_gayageum[index].Dequeue();
            RhythmGameOnSelectedSheetPjw.Instance.unity_editor_current_scales_gameobject_gayageum[index].Dequeue();
        }
        else if (StaticDataPjw.is_banghyang_selected == true)
        {
            index = banghyang_scale_tag_between_queue_index[tag];
            RhythmGameOnBanghyangPjw.Instance.unity_editor_current_scales_banghyang[index].Dequeue();
            RhythmGameOnBanghyangPjw.Instance.unity_editor_current_scales_gameobject_banghyang[index].Dequeue();
        }
        else //장구
        {

        }
    } 
}

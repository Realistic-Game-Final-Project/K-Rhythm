using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAndUpdatingQueuePjw : MonoBehaviour
{
    private Dictionary<string, int> gayageum_scale_tag_between_queue_index = new Dictionary<string, int>();
    private void Awake()
    {
        gayageum_scale_tag_between_queue_index.Add("end_3", (int)GAYAGEUM_SCALE_NUMBER.ONE);
        gayageum_scale_tag_between_queue_index.Add("end_4", (int)GAYAGEUM_SCALE_NUMBER.TWO);
        gayageum_scale_tag_between_queue_index.Add("end_5", (int)GAYAGEUM_SCALE_NUMBER.THREE);
        gayageum_scale_tag_between_queue_index.Add("end_6", (int)GAYAGEUM_SCALE_NUMBER.FOUR);
        gayageum_scale_tag_between_queue_index.Add("end_7", (int)GAYAGEUM_SCALE_NUMBER.FIVE);
        gayageum_scale_tag_between_queue_index.Add("end_8", (int)GAYAGEUM_SCALE_NUMBER.SIX);
        gayageum_scale_tag_between_queue_index.Add("end_9", (int)GAYAGEUM_SCALE_NUMBER.SEVEN);
        gayageum_scale_tag_between_queue_index.Add("end_10", (int)GAYAGEUM_SCALE_NUMBER.EIGHT);
        gayageum_scale_tag_between_queue_index.Add("end_11", (int)GAYAGEUM_SCALE_NUMBER.NINE);        
    }

    //TODO : 지금 사용자가 누르지 않았을 때 collision 부분과 겹쳐져서 , 제거 되는 코드가 에러가 있어서
    //일단 collision 검사 부분을 뒤로 밀어 버리는 방식을 썼으나 이는 임시 방편일 뿐 고쳐야 함.
    //SOL : 그냥 여기 지나면 삭제
    private void OnTriggerExit2D(Collider2D collision)
    {
        PopFromSelectedQueue(gameObject.tag);        
        Destroy(collision.gameObject);
    }

       private void PopFromSelectedQueue(string tag)
    {       
        int index = gayageum_scale_tag_between_queue_index[tag];  
        RhythmGameOnSelectedSheetPjw.Instance.unity_editor_current_scales[index].Dequeue();
        RhythmGameOnSelectedSheetPjw.Instance.unity_editor_current_scales_gameobject[index].Dequeue();
    } 
}

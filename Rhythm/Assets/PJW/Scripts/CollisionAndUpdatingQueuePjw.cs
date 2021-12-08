using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAndUpdatingQueuePjw : MonoBehaviour
{
    private Dictionary<string, int> gayageum_scale_tag_between_queue_index = new Dictionary<string, int>();
    private void Awake()
    {
        gayageum_scale_tag_between_queue_index.Add("end_3", (int)GAYAGEUM_SCALE_NUMBER.FOUR);
        gayageum_scale_tag_between_queue_index.Add("end_4", (int)GAYAGEUM_SCALE_NUMBER.FIVE);
        gayageum_scale_tag_between_queue_index.Add("end_5", (int)GAYAGEUM_SCALE_NUMBER.SIX);
        gayageum_scale_tag_between_queue_index.Add("end_6", (int)GAYAGEUM_SCALE_NUMBER.SEVEN);
        gayageum_scale_tag_between_queue_index.Add("end_7", (int)GAYAGEUM_SCALE_NUMBER.EIGHT);
        gayageum_scale_tag_between_queue_index.Add("end_8", (int)GAYAGEUM_SCALE_NUMBER.NINE);
        gayageum_scale_tag_between_queue_index.Add("end_9", (int)GAYAGEUM_SCALE_NUMBER.TEN);
        gayageum_scale_tag_between_queue_index.Add("end_10", (int)GAYAGEUM_SCALE_NUMBER.ELEVEN);
        gayageum_scale_tag_between_queue_index.Add("end_11", (int)GAYAGEUM_SCALE_NUMBER.TWELVE);        
    }

    //TODO : ���� ����ڰ� ������ �ʾ��� �� collision �κа� �������� , ���� �Ǵ� �ڵ尡 ������ �־
    //�ϴ� collision �˻� �κ��� �ڷ� �о� ������ ����� ������ �̴� �ӽ� ������ �� ���ľ� ��.
    //SOL : �׳� ���� ������ ����
    private void OnTriggerExit2D(Collider2D collision)
    {
        PopFromSelectedQueue(gameObject.tag);        
        Destroy(collision.gameObject);
    }

    private void PopFromSelectedQueue(string tag)
    {       
        int index = gayageum_scale_tag_between_queue_index[tag];  
        RhythmGameOnSelectedSheetPjw.Instance.unity_editor_current_scales_gayageum[index].Dequeue();
        RhythmGameOnSelectedSheetPjw.Instance.unity_editor_current_scales_gameobject_gayageum[index].Dequeue();
    } 
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//12/13 
//�� �ڵ带 �׽�Ʈ �غ� ���
//���⼭ ������ �Ͼ�� ���� collision�� ������ �ȵǼ�
//�����Ͱ� ������� �ʴ� ��� �� ��
//������ �ڷᱸ���� �ùٸ��� ����.
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

    //TODO : ���� ����ڰ� ������ �ʾ��� �� collision �κа� �������� , ���� �Ǵ� �ڵ尡 ������ �־
    //�ϴ� collision �˻� �κ��� �ڷ� �о� ������ ����� ������ �̴� �ӽ� ������ �� ���ľ� ��.
    //SOL : �׳� ���� ������ ����
    private void OnTriggerExit2D(Collider2D collision)
    {
        int index = gayageum_scale_tag_between_queue_index[tag];
        //Debug.Log("�ڷᱸ��" + gameObject.tag +"  " + index + "���� ����");
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
        else //�屸
        {

        }
    } 
}

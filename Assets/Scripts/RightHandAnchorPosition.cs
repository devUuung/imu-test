using UnityEngine;

public class RightHandAnchorPosition : MonoBehaviour
{
    public Transform RightHandAnchor; // RightHandAnchor ������Ʈ�� Transform

    void Update()
    {
        if (RightHandAnchor != null)
        {
            // RightHandAnchor�� ��ġ�� ������
            Vector3 position = RightHandAnchor.position;
            // ����: ���� ������Ʈ�� ��ġ�� RightHandAnchor ��ġ�� ����
            transform.position = position;
        }
    }
}

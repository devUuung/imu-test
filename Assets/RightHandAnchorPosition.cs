using UnityEngine;

public class RightHandAnchorPosition : MonoBehaviour
{
    public Transform RightHandAnchor; // RightHandAnchor 오브젝트의 Transform

    void Update()
    {
        if (RightHandAnchor != null)
        {
            // RightHandAnchor의 위치를 가져옴
            Vector3 position = RightHandAnchor.position;
            // 예시: 현재 오브젝트의 위치를 RightHandAnchor 위치로 설정
            transform.position = position;
        }
    }
}

using UnityEngine;

public class RightHandAnchorPosition : MonoBehaviour
{
    public Transform RightHandAnchor; // RightHandAnchor의 Transform

    void Update()
    {
        if (RightHandAnchor != null)
        {
            // RightHandAnchor의 위치를 가져옴
            Vector3 position = RightHandAnchor.position;
            // 방향: RightHandAnchor의 위치에 따라 이 객체의 위치를 설정
            transform.position = position;
            transform.rotation = RightHandAnchor.rotation; // 방향도 따라가게 설정
        }
    }
}

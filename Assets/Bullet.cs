using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 20f;

    // 초기 속도를 설정할 변수
    private Vector3 initialVelocity;

    // 속도 설정 함수
    public void SetVelocity(Vector3 velocity)
    {
        initialVelocity = velocity;
    }

    void Start()
    {
        // Rigidbody를 이용해 초기 속도 적용
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = initialVelocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.SendMessage("OnHitBullet", SendMessageOptions.DontRequireReceiver);
        Destroy(gameObject);
    }
}

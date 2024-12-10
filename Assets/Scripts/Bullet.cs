using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 20f;

    // �ʱ� �ӵ��� ������ ����
    private Vector3 initialVelocity;

    // �ӵ� ���� �Լ�
    public void SetVelocity(Vector3 velocity)
    {
        initialVelocity = velocity;
    }

    void Start()
    {
        // Rigidbody�� �̿��� �ʱ� �ӵ� ����
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = initialVelocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.SendMessage("OnHitBullet", SendMessageOptions.DontRequireReceiver);
        Destroy(gameObject);
    }
}

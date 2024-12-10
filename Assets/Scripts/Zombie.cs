using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{

    [SerializeField] public float moveSpeed = 0f;
    [SerializeField] float lifetime = 3f;
    [SerializeField] ParticleSystem HitParticle;
    private Animator animator;
    private bool isHit = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        Collider collider = GetComponent<Collider>();
    }

    void Update()
    {
        if (!isHit)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            GameObject microphone = GameObject.Find("flag");
            transform.LookAt(microphone.transform.position);
            if (!stateInfo.IsName("Z_Idle") && !stateInfo.IsName("Z_FallingBack"))
            {
                if (microphone != null)
                {
                    Vector3 direction = (microphone.transform.position - transform.position).normalized;
                    transform.position += direction * moveSpeed * Time.deltaTime;
                }
            }
        }
    }
    void OnHitBullet()
    {
        if (!isHit)
        {
            isHit = true;
            animator.Play("Z_FallingBack");
            HitParticle.Play();
            Destroy(gameObject, lifetime);
            Destroy(GetComponent<Collider>());
        }
    }
}


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

    public GameObject microphone;

    void Start()
    {
        animator = GetComponent<Animator>();
        Collider collider = GetComponent<Collider>();
    }

    void Update()
    {
        // 좀비가 맞지 않았을 경우에만 동작
        if (!isHit)
        {
            // "flag"라는 이름의 게임 오브젝트를 찾음
            microphone = GameObject.Find("flag");
            // 마이크가 존재할 경우
            if (microphone != null)
            {
                // 현재 애니메이터의 상태 정보를 가져옴
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                // 좀비가 마이크를 바라보도록 회전
                transform.LookAt(microphone.transform.position);
                // 현재 애니메이션 상태가 "Z_Idle" 또는 "Z_FallingBack"이 아닐 경우
                if (!stateInfo.IsName("Z_Idle") && !stateInfo.IsName("Z_FallingBack"))
                {
                    // 마이크 방향으로 이동
                    Vector3 direction = (microphone.transform.position - transform.position).normalized;
                    transform.position += direction * moveSpeed * Time.deltaTime; // 이동 속도에 따라 위치 업데이트
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


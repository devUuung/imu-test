using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] float lifetime = 0f;
    private bool isHit = false;
    // Start is called before the first frame update
    void Start()
    {
        Collider collider = GetComponent<Collider>();
    }

    // Update is called once per frame
  
    void OnHitBullet()
    {
        if (!isHit)
        {
            isHit = true;
            Destroy(gameObject, lifetime);
        }
    }
}

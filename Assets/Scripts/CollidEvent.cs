using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidEvent : MonoBehaviour
{
    public GameObject TimeCanvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // OnTriggerEnter 메서드 추가
    void OnTriggerEnter(Collider other) 
    {
        TimeCanvas.SetActive(false);
    }
}


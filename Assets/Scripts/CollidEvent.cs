using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidEvent : MonoBehaviour
{
    public GameObject TimeCanvas;
    public GameObject FailCanvas;
    public GameObject ZombieSpawner;
    // OnTriggerEnter 메서드 추가
    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.name == "Zombie1(Clone)") 
        {
            Destroy(other.gameObject);
            TimeCanvas.SetActive(false);
            FailCanvas.SetActive(true);
            ZombieSpawner.SetActive(false);
        }
    }
}


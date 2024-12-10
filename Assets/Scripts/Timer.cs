using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public GameObject TimeCanvas;
    public GameObject ClearCanvas;
    public GameObject ZombieSpawner;
    private float elapsedTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        timerText.text = elapsedTime.ToString("F2") + "s";

        if (elapsedTime >= 30f)
        {
            elapsedTime = 0f;
            TimeCanvas.SetActive(false);
            ClearCanvas.SetActive(true);
            ZombieSpawner.SetActive(false);
        }
    }
}

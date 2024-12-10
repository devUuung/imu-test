using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class MicrophoneInput : MonoBehaviour
{
    public AudioSource audioSource;
    public float decibel;
    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = Microphone.Start(null, true, 10, AudioSettings.outputSampleRate);
        while (!(Microphone.GetPosition(null) > 0)) { } // 마이크 입력 대기
        audioSource.loop = true;
        audioSource.Play();

        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }

    }

    private void Update()
    {
        float[] data = new float[256];
        float a = 0;
        audioSource.GetOutputData(data, 0);

        foreach (float s in data)
        {
            a += s * s;
        }
        float v1 = Mathf.Sqrt(a / data.Length); // 1번 마이크

        audioSource.GetOutputData(data, 1);

        foreach (float s in data)
        {
            a += s * s;
        }
        float v2 = Mathf.Sqrt(a / data.Length); // 2번 마이크

        decibel = 10000 * (v1 + v2);
    }
}

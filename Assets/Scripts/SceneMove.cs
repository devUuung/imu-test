using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    public void ToMain()
    {
        SceneManager.LoadScene(0);
    }

    public void ToPlay()
    {
        SceneManager.LoadScene(1);
    }

    public void ToAim()
    {
        SceneManager.LoadScene(2);
    }
}

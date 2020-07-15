using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        Time.timeScale = 1;
    }

    public void GamePause()
    {
        isPaused = !isPaused;

        if (isPaused)
            Time.timeScale = 0;

        if (!isPaused)
            Time.timeScale = 1;
    }

    void OnApplicationPause()
    {
        isPaused = true;
        Time.timeScale = 0;
    }
}
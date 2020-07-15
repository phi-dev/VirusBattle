using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public float initialTime;

    public void PlayArcadeMode()
    {
        SceneManager.LoadScene("Main");
    }

    public void PlayTimeMode()
    {
        PlayerPrefs.SetFloat("time", 20);
        PlayerPrefs.SetFloat("time_score", 0);
        PlayerPrefs.SetFloat("health",150);
        SceneManager.LoadScene("MainTime");
    }
}

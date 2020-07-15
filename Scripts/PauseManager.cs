using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class PauseManager : MonoBehaviour
{
    public GameObject panelPause;
    public VideoPlayer background;
    public Text countdownDisplay;
    private AudioSource myaudio;

    private int countdownTime;

    public void Pause()
    {
        panelPause.SetActive(true);
        Time.timeScale = 0;
        background.Pause();
    }

    public void Resume()
    {
        panelPause.SetActive(false);
        countdownDisplay.gameObject.SetActive(true);
        myaudio = GetComponent<AudioSource>();
        StartCoroutine(CountdownToResume());
    }

    IEnumerator CountdownToResume()
    {
        countdownTime = 3;
        while (countdownTime > 0)
        {
            countdownDisplay.text = countdownTime.ToString();

            yield return new WaitForSecondsRealtime(1f);

            countdownTime--;
        }

        countdownDisplay.text = "GO!";

        yield return new WaitForSecondsRealtime(1f);

        countdownDisplay.gameObject.SetActive(false);

        StopCoroutine(CountdownToResume());

        myaudio.UnPause();

        background.Play();

        Time.timeScale = 1;
    }
    
    public void onUserClickYesNoToMenu(int choise) //yes->choise==1, no->choise==0
    {
        if (choise == 1)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("StartMenù");
        }
    }

    public void onUserClickYesNoToRestart(int choice) //yes->choise==1, no->choise==0
    {
        if (choice == 1)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Main");
        } 
        else if(choice == 2)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("MainTime");
        }
    }
}

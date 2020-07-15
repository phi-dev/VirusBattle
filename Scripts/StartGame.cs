using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class StartGame : MonoBehaviour
{
    public VideoPlayer video;
    public GameObject button;
    private AudioSource audioSource;

    void Start()
    {
        PlayerPrefs.SetInt("hasPlayed",0);
        this.video.Prepare();
        StartCoroutine(this.active());
    }

    private IEnumerator active()
    {
        yield return new WaitForSeconds(2);
        this.button.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.video.isPlaying)
            videoStopPlaying();
    }

    public void videoStopPlaying()
    {
        SceneManager.LoadScene("StartMenù");
    }
}

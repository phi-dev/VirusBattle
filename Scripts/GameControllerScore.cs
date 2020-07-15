using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class GameControllerScore : GameController
{
    public Text scoreText;
    private bool highScoreVisualized = false;

    public TextMeshProUGUI gameOverScoreText;
    public TextMeshProUGUI gameOverRecordScoreText;

    public int hazardCountMax; /* numero di hazards max */

    public TextMeshProUGUI newHighScoreText;

    public override void Start()
    {
        base.Start();

        this.score = 0;
        this.scoreText.text = "";
        UpdateScoreText();
    }

    public override void AddScore(int newScoreValue)
    {
        base.AddScore(newScoreValue);
        if (!highScoreVisualized) // se il testo dell'high non è stato mostrato
            if (score > PlayerPrefs.GetInt("highScore")) // se ho superato il record
            {
                highScoreVisualized = true;
                StartCoroutine(ShowHighScore()); // mostra la scritta
            }
        UpdateScoreText();        
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    private void StopScore()
    {
        CancelInvoke("AddScore");

        /* MIGLIORARE: la key deve essere puttata al primo avvio del gioco */

        if (PlayerPrefs.HasKey("highScore"))
        {
            if (score > PlayerPrefs.GetInt("highScore")) /** se non muori il punteggio non verrà salvato */
            {
                PlayerPrefs.SetInt("highScore", score);
                if (PlayerPrefs.HasKey("username"))
                    StartCoroutine(this.saveNewHighScore(score));
            }
        }
        else
        {
            PlayerPrefs.SetInt("highScore", score); 
            if (PlayerPrefs.HasKey("username"))
                StartCoroutine(this.saveNewHighScore(score));
        }
    }

    public override void GameOver()
    {
        base.GameOver();

        this.StopScore(); // aggiorna il record score
        gameOverScoreText.text = "" + this.score;
        gameOverRecordScoreText.text = "" + PlayerPrefs.GetInt("highScore");
    }

    public override void RestartGame()
    {
        base.RestartGame();
    }

    public override void ReturnMenu()
    {
        base.ReturnMenu();
    }

    IEnumerator ShowHighScore()
    {
        newHighScoreText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        newHighScoreText.gameObject.SetActive(false);
        StopCoroutine(ShowHighScore());
    }

    public override int calcSpawnsNumber()
    {
        return Random.Range(hazardCountMin,hazardCountMax);
    }

    private IEnumerator saveNewHighScore(int score)
    {
        string url = "http://sharebox.altervista.org/insert_highscore.php";
        WWWForm form = new WWWForm();
        form.AddField("deviceId", SystemInfo.deviceUniqueIdentifier);
        form.AddField("score", score.ToString());

        Debug.Log("Saving new high score online");

        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Web error");
        }
    }

}

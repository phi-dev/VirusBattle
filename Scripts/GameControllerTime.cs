using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameControllerTime : GameController
{
    public Text timeText; /* testo che mostra il tempo rimanente */

    /* componenti grafiche che mostrano informazioni durante il game over, da spostare in gameOverController */
    public TextMeshProUGUI gameOverTimeText;
    public TextMeshProUGUI gameOverRecordTimeText;
    
    public float remainingTime; // tempo rimanente
    public float deltaRemainingTime; // quanto tempo incremento?
    private float maxTime;

    public int hazardCountDelta; /* numero di hazards */

    private const float LAST_LEVEL_TIME = 100; // tempo massimo 1 minuto e 40 secondi

    // pannello livello completato
    public GameObject levelCompletedPanel;

    // pannello livelli completati
    public GameObject allLevelsCompleted;
    
    public override void Start()
    {
        base.Start();

        this.maxTime = getMaximumTime();
        //this.remainingTime = 120; // for debugging
        //this.score = 3305; // for debugging
        this.remainingTime = maxTime;
        this.timeText.text = "";
        UpdateTimeText();        
    }

    private void Update()
    {
        if(!gameOver)
        {
            remainingTime -= Time.deltaTime; // aggiornamento tempo rimanente
            this.UpdateTimeText(); // aggiorna il contenuto del testo indicante il tempo rimanente
            if (levelWon())
            {
                this.GameLevelCompleted();
            }
        }
    }

    private void stopScore() /* memorizza lo score se questo è da record */
    {
        int totalScore = (int) PlayerPrefs.GetFloat("time_score");

        if (PlayerPrefs.HasKey("highTimeScore"))
        {
            if (totalScore > PlayerPrefs.GetInt("highTimeScore"))
            {
                PlayerPrefs.SetInt("highTimeScore", totalScore);
                // MEMORIZZA ANCHE IN DB ONLINE (se player ha un nome settato)
                if(PlayerPrefs.HasKey("username"))
                    StartCoroutine(this.saveNewHighTimeScore(totalScore));
            }
        }
        else
        {
            PlayerPrefs.SetInt("highTimeScore", totalScore);
        }
    }

    void UpdateTimeText()
    {
        timeText.text = "Time: " + (int)remainingTime;
    }

    public override void GameOver()
    {
        this.Resethealth(); // prima che venga distrutto il giocatore
        base.GameOver();

        this.increaseTimeScore(score);
        this.stopScore();

        // il punteggio totale accumulato non deve essere aggiornato, COSì COME IL TEMPO
        //Debug.Log(PlayerPrefs.GetFloat("time_score"));

        gameOverTimeText.text = "" + (int)PlayerPrefs.GetFloat("time_score");
        gameOverRecordTimeText.text = "" + PlayerPrefs.GetInt("highTimeScore");
        this.ResetTime(20);
    }

    public void GameLevelCompleted()
    {
        this.gameOver = true; // cambiare il nome della variabile
        PlayerPrefs.SetFloat("health", player.GetComponent<PlayerController>().GetHealth());
        this.CompleteGame(); // per far esplodere tutti

        if (this.isLastLevel())
        {
            this.GameOver();
            return;
        }

        this.levelCompletedPanel.SetActive(true); /* fa apparire il pannello di livello completato */

        /* INCREMENTO TEMPO E PUNTEGGIO ACCUMULATO */
        this.increaseTimeScore(score);
        this.increaseMaximumTime(deltaRemainingTime); /* viene incrementato il tempo rimanente */
    }

    private void increaseTimeScore(float score)
    {
        float time_score = PlayerPrefs.GetFloat("time_score"); /* trova il punteggio fatto fino ad ora nella modalità a tempo */
        PlayerPrefs.SetFloat("time_score", time_score + score); /* incrementa il punteggio */
    }

    public override void RestartGame()
    {
        this.levelCompletedPanel.SetActive(false);
        if (!levelWon())
            this.Resethealth();

        base.RestartGame();
    }

    public override void ReturnMenu()
    {
        base.ReturnMenu();
    }

    /* il giocatore ha vinto nella modalità a tempo se ha esaurito il tempo del "livello" e allo stesso tempo
     * si trovava all'ultimo livello */
    private bool hasWonWholeGame()
    {
        return levelWon() && isLastLevel();
    }

    private bool isLastLevel()
    {
        return getMaximumTime() >= LAST_LEVEL_TIME;
    }
    
    /* il livello è da considerarsi vinto se il tempo è stato esaurito tutto */
    private bool levelWon()
    {
        return this.remainingTime <= 0;
    }

    public void ResetTime(float time) /* resetta il max time a zero*/
    {
        PlayerPrefs.SetFloat("time_score", 0);
        PlayerPrefs.SetFloat("time", time);
    }

    /* funzione per impostare il tempo massimo nel livello corrente */
    private void increaseMaximumTime(float increase)
    {
        if(!isLastLevel()) /* se non mi trovo all'ultimo livello allora non incrementare */
            PlayerPrefs.SetFloat("time", PlayerPrefs.GetFloat("time") + increase);
    }

    private float getMaximumTime()
    {
        return (PlayerPrefs.GetFloat("time")==0)?20:PlayerPrefs.GetFloat("time");
    }

    public void Resethealth()
    {
        // resetta la vita al massimo
        PlayerPrefs.SetFloat("health",player.GetComponent<PlayerController>().maxHealth);
    }

    public override int calcSpawnsNumber()
    {
        return hazardCountMin + hazardCountDelta * waveNumber;
    }

    private IEnumerator saveNewHighTimeScore(int score)
    {
        string url = "http://sharebox.altervista.org/insert_hightimescore.php";
        WWWForm form = new WWWForm();
        form.AddField("deviceId",SystemInfo.deviceUniqueIdentifier);
        form.AddField("score",score.ToString());
                
        UnityWebRequest www = UnityWebRequest.Post(url,form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Web error");
        }        
    }
}

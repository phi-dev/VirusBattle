using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum EnemyType
{
    BATTERIO,
    VIRUS_A,
    VIRUS_B
}

public abstract class GameController : MonoBehaviour
{
    public GameObject[] bonuses;

    public Vector3 spawnValues; /* valori per determinare le coordinate per lo spawning degli oggetti */
    public int hazardCountMin; /* numero di hazards */
    public float spawnWait; /* tempo di attesa tra uno spawn e l'altro */
    public float minSpawnWait;
    public float deltaSpawnWait;
    public float startWait; /* dopo quanto tempo iniziare il gioco */
    public float waveWait; /* tempo di attesa tra un'ondata e la successiva */
        
    protected bool gameOver;
    private bool restart;

    public int destroyedEnemies;

    public GameObject player;
    public GameObject playerBody;

    protected int score;
    
    /* da mettere in health bar controller */
    public GameObject healthBar;
    public TextMeshProUGUI healthText;

    /*Barra che mostra le informazioni sul gioco*/
    public GameObject gameInfoPanel;

    /* Pannello gameover */
    public GameObject gameOverPanel;

    /* Bottone per mettere in pausa*/
    public GameObject pauseButton;
    
    /* conta a quale ondata mi trovo */
    public int waveNumber;

    /* probabilità di ricevere il bonus */
    public int bonusProbability;

    public float minLifeBonusHealth;

    /* da togliere */
    public GameObject explosion;
    
    /*lista dei nemici attualmente spawnati e presenti sulla scena di gioco*/
    private ArrayList currentHazards = new ArrayList();

    public PauseManager pauseManager;

    private WaveSettings[] waveSettings;
    
    public virtual void Start()
    {
        gameOver = false;
        restart = false;

        this.destroyedEnemies = 0;
        
        /* la barra della vita deve essere in mostra */
        this.gameInfoPanel.SetActive(true);

        /* nasconde il pannello del gameover */
        this.gameOverPanel.SetActive(false);

        this.waveSettings = gameObject.GetComponent<WaveSpawnerSettings>().settings;

        StartCoroutine(SpawnWaves());
    }
    
    protected IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait); // attesa di n secondi per spawnare il primo nemico
        while (true)
        {
            bool globuloSpawned = false; // nell'ondata corrente nessun globulo spawnato
            bool drugSpawned = false; // se è stato spawnato un farmaco nell'ondata corrente

            bool bonusSpawned = false;

            currentHazards.Clear(); /* pulizia della lista degli hazards: deve essere pulita ad ogni ciclo, altrimenti
                                        diventa troppo grande e occupa troppa memoria */

            int extractedNumberForBonus = Random.Range(0, bonusProbability);
            bool shouldSpawnBonus = extractedNumberForBonus == 1;
            Debug.Log(extractedNumberForBonus);

            /* ciclo per spawnaggio nemici */
            for (int i = 0; i < calcSpawnsNumber() && !gameOver; i++)
            {
                GameObject[] enemies = GetWaveEnemies(); // trova il gruppo di nemici da istanziare
                /* blocco per istanziare un nuovo nemico */
                GameObject hazard = enemies[Random.Range(0, enemies.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                currentHazards.Add(Instantiate(hazard, spawnPosition, spawnRotation));
                
                yield return new WaitForSeconds(spawnWait);

                /* se nel ciclo corrente non è stato spawnato alcun bonus */
                if(/*!globuloSpawned && !drugSpawned*/!bonusSpawned && shouldSpawnBonus)
                {
                    int bonusId = spawnBonus();
                    /*if (bonusId == 0) globuloSpawned = true;
                    else if (bonusId == 1) drugSpawned = true;
                    else if (bonusId == 2) drugSpawned = true;*/
                    if (bonusId >= 0)
                        bonusSpawned = true;

                    yield return new WaitForSeconds(spawnWait);
                }

            }
            if(spawnWait>=minSpawnWait)
                spawnWait -= deltaSpawnWait;

            yield return new WaitForSeconds(waveWait);

            this.waveNumber++;
            
            if (gameOver)
            {
                restart = true;
                break;
            }
        }
    }

    public abstract int calcSpawnsNumber();

    private GameObject[] GetWaveEnemies()
    {
        GameObject[] enemies = null;
        foreach(WaveSettings ws in this.waveSettings)
        {
            if(this.waveNumber >= ws.wave)
            {
                enemies = ws.hazards;
            }
        }
        return enemies;
    }

    private int spawnBonus()
    {
        if (player == null)
            return -1;

        int num = Random.Range(0, bonusProbability);
        Quaternion spawnRotation = Quaternion.identity;

        float playerHealth = player.GetComponent<PlayerController>().GetHealth();
        int begin = (playerHealth <= minLifeBonusHealth) ? 0 : bonuses.Length-1;

        /* se health giocatore <=40 allora spawna solo sparo doppio, altrimenti spawna anche la vita */
        int bonusId = Random.Range(begin,bonuses.Length);

        /* blocco per spawnaggio del bonus */
        Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
        currentHazards.Add(Instantiate(bonuses[bonusId], spawnPosition, spawnRotation));
        return bonusId;
    }
        
    public void CompleteGame() // cosa accade se il gioco è stato completato -> viene richiamato sia in game over che quando si vince
    {
        /* ciclo per distruzione ed esplosione dei nemici */
        foreach (GameObject enemyGo in this.currentHazards)
        {
            if (enemyGo != null && enemyGo.GetComponent<EnemyController>() != null)
            {
                enemyGo.GetComponent<EnemyController>().explode();
            }
        }

        /* distruzione del giocatore, spostare in playercontroller */
        this.player.GetComponent<PlayerController>().explode();
        this.player.SetActive(false);
        //Destroy(this.player); // non serve a nulla
    }
    
    public virtual void GameOver()
    {
        gameOver = true;
        this.CompleteGame();

        /* blocco per gestione componenti grafiche/pannelli */
        this.gameInfoPanel.SetActive(false);
        this.pauseButton.SetActive(false);
        this.gameOverPanel.SetActive(true);
    }

    public void OnApplicationPause()
    {
        if(!gameOver)
            pauseManager.Pause();
    }


    public virtual void RestartGame()
    {
        Time.timeScale = 1;
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }

    public virtual void ReturnMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartMenù");
    }

    public virtual void AddScore(int newScoreValue)
    {
        score += newScoreValue;
    }
}

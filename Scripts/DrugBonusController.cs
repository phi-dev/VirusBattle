using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* script per abilitare lo sparo doppio */
public class DrugBonusController : MonoBehaviour
{
    private GameController gameController;

    public GameObject powerUp;

    public float bonusTime;

    public void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("GameController script not found");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        /* quando entra a contatto con il giocatore, deve fornirgli vita e poi sparire */
        if (other.gameObject.tag == "Player")
        {
            Instantiate(powerUp);
            int n = Random.Range(0, 3);
            if (n == 0)
            {
                gameController.player.GetComponent<PlayerController>().enableTripleShot(bonusTime);
            }
            else
            {
                gameController.player.GetComponent<PlayerController>().enableDoubleShot(bonusTime);
            }
            Destroy(gameObject);
        }
    }
}

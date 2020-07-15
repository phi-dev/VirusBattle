using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusController : MonoBehaviour
{
    // Start is called before the first frame update
    public float health;

    private GameController gameController;

    public GameObject powerUp;

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
        if(other.gameObject.tag == "Player")
        {
            Instantiate(powerUp);
            gameController.player.GetComponent<PlayerController>().UpdateHealth(health);
            Destroy(gameObject);
        }
    }
}

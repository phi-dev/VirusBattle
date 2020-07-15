using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByBoundary : MonoBehaviour
{
    private GameController gameController;

    private void Start()
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

    void OnTriggerExit(Collider other)
    {
        /* se è un nemico a oltrepassare il bordo, questo deve far perdere vita al giocatore se è boundarySensible */
        if(other.gameObject.tag=="Enemy" && other.gameObject.GetComponent<EnemyController>() != null)
        {
            if (other.gameObject.GetComponent<EnemyController>().boundarySensible)
            {
                gameController.player.GetComponent<PlayerController>().UpdateHealth(
                    -other.gameObject.GetComponent<EnemyController>().playerDamage / 2
                    ); // al giocatore è tolta salute pari a metà del danno che viene inflitto normalmente
            }
        }

        /* qualcunque cosa sia a toccare il bordo, questa deve essere distrutta */
        Destroy(other.gameObject);
    }
}

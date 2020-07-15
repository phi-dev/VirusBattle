using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ContactController : MonoBehaviour
{
    /**
     * Questo script ha il solo compito di abbassare/alzare la vita
         */

    /* esplosione dell'oggetto nemico (virus/batterio), null se è del globulo */
    public GameObject explosion;


    public GameObject playerExplosion;

    /* quanti punti aggiungere/togliere alla barra salute */
    public float healthPoints;
    
    /* true se deve avvenire un aggiornamento della vita nel caso in cui l'oggetto tocca il boundary */
    public bool boundarySensible;

    private GameController gameController;
    

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
        if (other.tag == "Boundary" || other.tag == "Enemy" || other.tag == "Laser")
        {
            return;
        }
                
        float newHealth = gameController.player.GetComponent<PlayerController>().UpdateHealth(healthPoints);
        /*if (other.tag == "Player" && gameController.player.GetComponent<PlayerController>().health <= 0)
        {
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            Destroy(other.gameObject);
            gameController.GameOver();
        }*/
    }

    /* Cosa fare quando nave nemica esce da boundary */
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy" || other.tag == "Player" || other.tag == "Laser")
            return;

        if (other.tag == "Boundary")
        {
            if (boundarySensible == false)
            {
                return;
            }
            //Debug.Log("Enemy hit boundary " + gameObject.tag);
            healthPoints /= 2;
        }

        float newHealth = gameController.player.GetComponent<PlayerController>().UpdateHealth(healthPoints);
        /*if (other.tag == "Player" && gameController.player.GetComponent<PlayerController>().health <= 0)
        {
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            Destroy(other.gameObject);
            gameController.GameOver();
        }*/
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public float initialSpeed;
    public float deltaSpeed; // di quanto aumenta la velocità ad ogni ondata
    public float maxSpeed;

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

        int wave = gameController.waveNumber;
        if (wave * deltaSpeed > initialSpeed - maxSpeed)
            deltaSpeed = (initialSpeed - maxSpeed)/wave;

        //Debug.Log("Speed: " + (initialSpeed - wave*deltaSpeed));
        GetComponent<Rigidbody>().velocity = transform.forward * (initialSpeed - wave*deltaSpeed);
    }
}

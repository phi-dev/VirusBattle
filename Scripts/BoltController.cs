using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoltController : MonoBehaviour
{
    public float damage;

    protected GameController gameController;
    GameObject gameControllerObject;

    public void Start()
    {
        gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("GameController script not found");
        }

    }

    public virtual void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject); // distrugge laser
    }
    
}

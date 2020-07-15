using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* script che descrive cosa succede quando il laser nemico colpisce il giocatore */
public class EnemyBoltController : BoltController
{
    public override void OnTriggerEnter(Collider other)
    {
        /* un laser può solo collidere con il player o con il laser del player */
        if(other.gameObject.tag != "Player")
        {
            return;
        }

        /* toglie vita al giocatore */
        PlayerController playerController = gameController.player.GetComponent<PlayerController>();
        playerController.UpdateHealth(-damage);

        /*poi si comporta come tutti i laser*/
        base.OnTriggerEnter(other);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* lo script descrive cosa fa il laser del player quando questo colpisce un nemico */
public class PlayerBoltController : BoltController
{
    public override void OnTriggerEnter(Collider other)
    {
        /* se l'oggetto colpito è un nemico */
        if (other.gameObject.tag != "Enemy")
            return;

        /* Toglie vita al nemico, solo se questo non è un laser */
        EnemyController enemyController = other.gameObject.GetComponent<EnemyController>();
        if (enemyController != null)
            enemyController.UpdateHealth(-damage);

        /* poi si comporta come laser normale */
        base.OnTriggerEnter(other);
    }
}

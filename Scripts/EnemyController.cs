using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum difficulty
{
    EASY,HARD
}

public class EnemyController : MonoBehaviour
{
    public float health; /* quanta salute ha il nemico */
    public GameObject explosion;
    public GameObject playerExplosion;
    public int scoreValue; /* se il nemico viene colpito dal laser, quanto mi fornisce al punteggio? */
    public bool boundarySensible; /* se true allora quando colpisce il bordo fa perdere vita al giocatore, altrimenti no */

    public float playerDamage; /* se colpisco il player, quanta vita gli tolgo */

    private GameController gameController;

    public EnemyType enemyType;

    // Start is called before the first frame update
    void Start()
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

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boundary" || other.tag == "Enemy" || other.tag=="Bonus")
        {
            return;
        }

        if (other.tag == "Laser") /* cosa succede se il nemico è colpito da un laser */
        {
            /* l'oggetto corrente viene ricolorato per un decimo di secondo, per dare impressione di essere stato colpito */
            Color original = rendererTransform().GetComponent<Renderer>().material.color;
            if (!original.Equals(new Color(0, 0, 0, 0.1f)))
            {
                rendererTransform().GetComponent<Renderer>().material.color = new Color(0, 0, 0, 0.1f);
                StartCoroutine(Recolor(original));
            }
        } else if (other.tag == "Player") /* cosa succede se il nemico entra in contatto con la navicella */
        {
            gameController.player.GetComponent<PlayerController>().UpdateHealth(-playerDamage);
            this.explode();
        }
    }

    public void UpdateHealth(float damage)
    {
        this.health += damage;

        if (this.health <= 0)
        {
            this.explode();
        }
    }

    public void explode()
    {
        
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }


        gameController.destroyedEnemies++;
        gameController.AddScore(scoreValue);
        Destroy(gameObject);
    }

    IEnumerator Recolor(Color original)
    {
        yield return new WaitForSeconds(0.1f);

        rendererTransform().GetComponent<Renderer>().material.color = original;
    }

    private Transform rendererTransform()
    {
        string gameObjectName = gameObject.name;
        Transform renderer;

        if (gameObjectName.StartsWith("Virus"))
            renderer = gameObject.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0); // se virus
        else
            renderer = gameObject.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1);

        return renderer;
    }
}

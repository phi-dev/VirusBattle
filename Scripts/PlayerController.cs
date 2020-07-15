using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
    public GameObject shot;
    public GameObject shotSpawn1;
    public GameObject shotSpawn2;
    public GameObject shotSpawn3;

    public GameObject explosion;

    public GameObject robotMesh;

    public float fireRate; // Frequenza dello sparo
    private float nextFire;

    public float speed; // velocità che può assumere il giocatore, utile per spostamento a destra e sinistra
    public Boundary boundary; // bordo olter il quale il player non può andare
    public float tilt; // 

    public SimpleTouchPad touchPad; // no comment

    private float health;
    public float maxHealth;

    private GameController gameController;

    private float bonusTime;

    public bool loadFromPrefs;

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

        // to refactor
        if (loadFromPrefs)
        {
            this.health = PlayerPrefs.GetFloat("health");
        }
        else
        {
            this.health = maxHealth;
            PlayerPrefs.SetFloat("health", this.maxHealth);
        }

        this.updateHealthText();
    }

    void Update()
    {
        if (touchPad.CanFire() && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            //GameObject clone = 
            Instantiate(shot, shotSpawn1.transform.position, shotSpawn1.transform.rotation);// as GameObject; /*  */
            if(doubleShotEnabled())
                Instantiate(shot, shotSpawn2.transform.position, shotSpawn2.transform.rotation);// as GameObject; /*  */
            if (tripleShotEnabled())
            {
                Instantiate(shot, shotSpawn2.transform.position, shotSpawn2.transform.rotation);// as GameObject; /*  */
                Instantiate(shot, shotSpawn3.transform.position, shotSpawn3.transform.rotation);// as GameObject; /*  */
            }

            GetComponent<AudioSource>().Play();
        }

        /* se il doppio sparo è abilitato allora decrementa il tempo di disponibiltà del bonus */
        if (doubleShotEnabled() || tripleShotEnabled())
        {
            bonusTime -= Time.deltaTime;
            if (bonusTime <= 0)
                disableDoubleShot();
        }
    }
        
    void FixedUpdate()
    {
        Vector2 direction = touchPad.GetDirection();
        Vector2 pointerPosition = touchPad.GetPointerPosition();

        if (touchPad.HasGotFirstTouch())
            pointerPosition = Camera.main.ScreenToWorldPoint(pointerPosition);
        else
            pointerPosition.x = 0;

        Vector3 movement = new Vector3(((pointerPosition.x) - (gameObject.transform.position.x)), direction.y, 0);
        GetComponent<Rigidbody>().velocity = (movement * speed);
        GetComponent<Rigidbody>().position = new Vector3
          (
            Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
          );

        //robotMesh.transform.rotation = Quaternion.Euler(90f, GetComponent<Rigidbody>().velocity.x * (-tilt), GetComponent<Rigidbody>().velocity.x);
        robotMesh.transform.localRotation = Quaternion.Euler(0, GetComponent<Rigidbody>().velocity.x * (-tilt),0);
    }

    public void explode()
    {
        Instantiate(explosion, transform.position, transform.rotation);
    }
    
    public float UpdateHealth(float h)
    {
        this.health += h;

        if (this.health <= 0)
        {
            this.health = 0;
            this.gameController.GameOver();
        }
        else if (this.health > maxHealth)
        {
            this.health = maxHealth;
        }

        this.updateHealthText();
        return this.health;
    }

    public void updateHealthText()
    {
        this.gameController.healthText.text = ((int)this.health).ToString();
        this.gameController.healthBar.transform.localScale = new Vector3(this.health / maxHealth, 1, 1);
    }

    public float GetHealth()
    {
        return this.health;
    }

    public void SetHealth(float health)
    {
        this.health = health;
        this.updateHealthText();
    }

    public bool doubleShotEnabled()
    {
        return shotSpawn2.activeSelf && !shotSpawn3.activeSelf;
    }

    public bool tripleShotEnabled()
    {
        return shotSpawn2.activeSelf && shotSpawn3.activeSelf;
    }

    public void enableDoubleShot(float bonusTime)
    {
        this.disableBonuses();
        this.bonusTime = bonusTime;
        shotSpawn2.SetActive(true);

        float xPosition = gameObject.transform.position.x;
        float distancing = 0.06f;

        shotSpawn1.transform.position = new Vector3(xPosition + distancing, 0.0f, 0);
        shotSpawn2.transform.position = new Vector3(xPosition - distancing, 0.0f, 0);     
    }

    public void disableDoubleShot()
    {
        float xPosition = gameObject.transform.position.x;
        shotSpawn1.transform.position = new Vector3(xPosition, 0.0f, 0);
        shotSpawn2.SetActive(false);
    }

    public void enableTripleShot(float bonusTime)
    {
        this.disableBonuses();

        this.bonusTime = bonusTime;
        shotSpawn2.SetActive(true);
        shotSpawn3.SetActive(true);

        float angle = 3;


        //shotSpawn2.transform.localRotation = Quaternion.Euler(-90-angle - GetComponent<Rigidbody>().velocity.x * (-tilt), 90,-90);
        //shotSpawn3.transform.localRotation = Quaternion.Euler(-90+angle + GetComponent<Rigidbody>().velocity.x * (-tilt), 90,-90);
                
        shotSpawn2.transform.localRotation = Quaternion.Euler(-90-angle, 90,-90);
        shotSpawn3.transform.localRotation = Quaternion.Euler(-90+angle, 90,-90);

        shotSpawn1.transform.localPosition = new Vector3(0, 0, 0);
        shotSpawn2.transform.localPosition = new Vector3(0, 0, 0);
        shotSpawn3.transform.localPosition = new Vector3(0, 0, 0);

    }
    public void disableTripleShot()
    {
        shotSpawn2.transform.localRotation = Quaternion.Euler(-90,0,0);
        shotSpawn3.transform.localRotation = Quaternion.Euler(-90,0,0);
        shotSpawn2.SetActive(false);
        shotSpawn3.SetActive(false);
    }

    public void disableBonuses()
    {
        if (this.doubleShotEnabled())
            this.disableDoubleShot();
        if (this.tripleShotEnabled())
            this.disableTripleShot();
    }
}
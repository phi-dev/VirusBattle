using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image currentHealthBar;
    //public Text ratioText;

    private float hitpoint = 100;
    private float maxHitpoint = 100;

    private void Start()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float ratio = hitpoint / maxHitpoint;
        currentHealthBar.rectTransform.localScale = new Vector3(ratio, 1, 1);
        //ratioText.text = (ratio * 100).toString() + "%";
    }

    public void TakeDamage(float damage)
    {
        hitpoint -= damage;
        if(hitpoint<0)
        {
            hitpoint = 0;
            Debug.Log("Dead");
        }

        UpdateHealthBar();
    }

    public void HealDamage(float heal)
    {
        hitpoint += heal;
        if (hitpoint > maxHitpoint)
        {
            hitpoint = maxHitpoint;
        }

        UpdateHealthBar();
    }



}

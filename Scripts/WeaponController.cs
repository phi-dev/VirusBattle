using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;
    public float delay;
    public AudioClip[] clips;

    private AudioSource audioSource;
       
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        InvokeRepeating("Fire",delay,fireRate);
        Fire();
    }

    void Fire()
    {
        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        audioSource.clip = clip;
        audioSource.Play();
    }
}

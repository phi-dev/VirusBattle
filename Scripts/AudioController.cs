using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    public AudioMixer master;

    public void suondOff()
    {
        master.SetFloat("masterVolume", -80f);
    }

    public void soundOn()
    {
        master.SetFloat("masterVolume", PlayerPrefs.GetFloat("masterVolume"));
    }
}

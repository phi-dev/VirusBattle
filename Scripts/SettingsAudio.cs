using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsAudio : MonoBehaviour
{
    public AudioMixer master;
    //public Slider GlobalSlider;
    public Slider EffectsSlider;
    public Slider MusicSlider;

    void Start()
    {
        //GlobalSlider.value = PlayerPrefs.GetFloat("masterVolume");
        EffectsSlider.value = PlayerPrefs.GetFloat("effectsVolume");
        MusicSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    /*
    public void setGlobalVolume(float volume)
    {
        master.SetFloat("masterVolume", Mathf.Log(volume)*20);
        PlayerPrefs.SetFloat("masterVolume", volume);
        PlayerPrefs.Save();
    }
    */

    public void setMusicVolume(float volume)
    {
        master.SetFloat("musicVolume", Mathf.Log(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
        PlayerPrefs.Save();
    }

    public void setEffectsVolume(float volume)
    {
        master.SetFloat("effectsVolume", Mathf.Log(volume) * 20);
        PlayerPrefs.SetFloat("effectsVolume", volume);
        PlayerPrefs.Save();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveSettings
{
    public int wave;
    public GameObject[] hazards;
}

public class WaveSpawnerSettings : MonoBehaviour
{
    public WaveSettings[] settings;
}

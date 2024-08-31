using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Grave : MonoBehaviour
{
    // Start is called before the first frame update
    public float graveRespawnTime = 10.0f;
    public float graveSpawnDelay = 10.0f;
    public float graveMaxSpawnDelay = 10.0f;
    public float graveMaxRespawnTime = 10.0f;
    public float graveDigTime = 2.0f;

    public void Dig()
    {
        Debug.Log("Grave digged");
        graveRespawnTime += Math.Min(graveDigTime, graveMaxRespawnTime);
    }
}
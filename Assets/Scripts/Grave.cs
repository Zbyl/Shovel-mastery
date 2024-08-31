using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Grave : MonoBehaviour
{
    // Start is called before the first frame update
    public float graveRespawnTime = 10.0f;
    public float graveMaxRespawnTime = 10.0f;
    public float graveDigTime = 2.0f;

    private Transform zombiesRoot;

    public Zombie zombiePrefab;
    void Start()
    {
        
        zombiesRoot = GameObject.Find("Zombies").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (graveRespawnTime > 0)
        {
            graveRespawnTime -= Time.deltaTime;
            if (graveRespawnTime <= 0)
            {
                Zombie newObject = Instantiate(zombiePrefab, GetComponent<Transform>().position, GetComponent<Transform>().rotation);
                newObject.transform.parent = this.zombiesRoot;
            }
        }
        else
        {
            graveRespawnTime = graveMaxRespawnTime;
        }
    }

    public void Dig()
    {
        Debug.Log("Grave digged");
        graveRespawnTime += Math.Min(graveDigTime, graveMaxRespawnTime);
    }
}
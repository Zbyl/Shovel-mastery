using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    private List<Grave> graves;

    public int maxZombies = 8;
    private int zombiesCounter = 0;
    private float delay = 5.0f;

    private Transform zombiesRoot;

    public Zombie zombiePrefab;

    void Start()
    {
        zombiesRoot = GameObject.Find("Zombies").transform;
        graves = new List<Grave>(GameObject.FindObjectsOfType<Grave>());
    }

    void Update()
    {
        zombiesCounter = new List<Zombie>(GameObject.FindObjectsOfType<Zombie>()).Count;
        delay -= Time.deltaTime;

        if (delay <= 0 && zombiesCounter < maxZombies)
        {
            delay = 5.0f;
            Grave grave = graves[Random.Range(0, graves.Count)];
            grave.OpenGrave();
            SpawnZombie(grave);
        }
    }

    void SpawnZombie(Grave grave)
    {
        Zombie newObject = Instantiate(zombiePrefab, grave.GetComponent<Transform>().position, grave.GetComponent<Transform>().rotation);
        newObject.transform.parent = this.zombiesRoot;
    }
}

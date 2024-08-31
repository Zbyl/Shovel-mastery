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
            List<Grave> unsealed_graves = graves.FindAll(grave => !grave.grave_sealed);
            if (unsealed_graves.Count == 0) return;

            Grave grave = graves[Random.Range(0, unsealed_graves.Count)];
            grave.OpenGrave();
            SpawnZombie(grave);
        }
    }

    void SpawnZombie(Grave grave)
    {
        Zombie newObject = Instantiate(zombiePrefab, grave.spawnPoint.position, grave.spawnPoint.rotation);
        newObject.transform.parent = this.zombiesRoot;
    }
}

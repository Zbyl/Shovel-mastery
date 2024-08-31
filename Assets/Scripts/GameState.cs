using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance { get; private set; }

    GameObject player;
    public ZombieSoundPlayer zombieSoundPlayerPrefab; // Prefab for zombie sound player.
    Dictionary<Zombie, ZombieSoundPlayer> zombieSoundPlayers = new Dictionary<Zombie, ZombieSoundPlayer>();

    public int tempCounter = 0;
    public bool isPaused = true;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        tempCounter += 1;
        removeOldZombieSoundPlayers();
    }

    void removeOldZombieSoundPlayers()
    {
        var toRemove = new List<Zombie>();
        foreach (var item in zombieSoundPlayers)
        {
            if (!item.Value.isPlaying())
            {
                //Debug.Log("Removing sound", item.Value);
                Destroy(item.Value.gameObject);
                toRemove.Add(item.Key);
            }
        }

        foreach (var item in toRemove)
        {
            zombieSoundPlayers.Remove(item);
        }
    }

    public float maxZombieSoundDistance = 10.0f;
    public float minZombieSoundInterval = 0.5f;
    public float maxZombieSoundInterval = 3.0f;
    float nextZombieSoundTime = 0.0f;               /// When to play next zombie sound.

    /// Plays a sound for a zombie every now and then.
    /// Called by zombies.
    public void ZombieSoundTick(Zombie zombie)
    {
        if (Time.time < nextZombieSoundTime) return;

        if (zombieSoundPlayers.ContainsKey(zombie)) return;

        var distance = (zombie.transform.position - player.transform.position).magnitude;
        if (distance > maxZombieSoundDistance) return;

        PlayZombieSound(zombie);
        nextZombieSoundTime = Time.time + Random.Range(minZombieSoundInterval, maxZombieSoundInterval);
    }

    private void PlayZombieSound(Zombie zombie)
    {
        var zombieSoundPlayer = Instantiate(zombieSoundPlayerPrefab, zombie.transform.position, zombie.transform.rotation);
        zombieSoundPlayers.Add(zombie, zombieSoundPlayer);
        zombieSoundPlayer.Play();
    }
}

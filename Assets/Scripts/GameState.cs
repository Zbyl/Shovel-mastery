using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance { get; private set; }

    GameObject player;
    public ZombieSoundPlayer zombieSoundPlayerPrefab; // Prefab for zombie sound player.
    Dictionary<Zombie, ZombieSoundPlayer> zombieSoundPlayers = new Dictionary<Zombie, ZombieSoundPlayer>();

    public bool isPaused = true;
    public static int playerMaxHealth = 5;
    public int playerHealth = playerMaxHealth;
    public int skeletonsKilled = 0;
    public float startTime = 0;
    public int gravesNumber = 0;
    public float powerShovelStrength = 0; // Between 0 and 1.

    public enum GameResult
    {
        PLAYING,
        WON,
        LOST,
    }
    public GameResult gameResult = GameResult.PLAYING;

    public AudioSource healingSound; // Used by healing flower. Kept here for simplicity.
    public AudioSource hitSound;

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
        startTime = Time.time;
        player = GameObject.Find("Player");
        healingSound = GameObject.Find("HealingSound").GetComponent<AudioSource>();
        hitSound = GameObject.Find("HitSound").GetComponent<AudioSource>();
        gravesNumber = GameObject.FindGameObjectsWithTag("Grave").Length;
        playerHealth = playerMaxHealth;
    }

    void Update()
    {
        if (skeletonsKilled >= gravesNumber && gameResult == GameResult.PLAYING)
        {
            gameResult = GameResult.WON;
        }

        if (playerHealth <= 0 && gameResult == GameResult.PLAYING)
        {
            gameResult = GameResult.LOST;
        }
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

    public void TakeHit(int v)
    {
        Debug.Log($"Player hit{Random.Range(0, 1023442342342340)}");
        playerHealth -= 1;
        hitSound.Play();
    }
}


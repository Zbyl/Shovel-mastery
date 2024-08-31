using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingFlower : MonoBehaviour
{
    public GameObject healthPickupParticlesPrefab;

    void OnTriggerEnter(Collider collider)
    {
        if (!collider.transform.CompareTag("Player")) return;
        if (GameState.Instance.playerHealth >= GameState.playerMaxHealth) return;
        GameState.Instance.playerHealth = GameState.playerMaxHealth;

        var particles = Instantiate(healthPickupParticlesPrefab, transform.position, transform.rotation, transform.parent);
        GameState.Instance.healingSound.Play();
        Destroy(gameObject);
    }
}

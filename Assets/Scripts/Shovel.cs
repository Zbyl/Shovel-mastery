using System;
using UnityEngine;

public class Shovel : MonoBehaviour
{
    public float damage = 10.0f;      // Damage dealt by the pickaxe
    public float attackRange = 3.5f;  // Range of the pickaxe attack
    public float attackRate = 1.0f;   // Time between attacks
    public Camera playerCamera;       // Reference to the player's camera

    private float nextAttackTime = 0f;

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextAttackTime)
        {
            Debug.Log("Attacking with shovel");
            Attack();
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    void Attack()
    {
        Debug.Log("Shovel attack!");
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, attackRange))
        {
            Zombie enemy = hit.transform.GetComponent<Zombie>();
            Debug.Log($"Hit: {hit.transform.name}");
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            // Optionally add some attack effects or animations here
        }
    }
}

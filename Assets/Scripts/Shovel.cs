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
            Attack();
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    void Attack()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, attackRange))
        {
            Zombie enemy = hit.transform.GetComponent<Zombie>();
            Debug.Log($"Hit: {hit.transform.name}");
            Vector3 pushDirection = hit.point - playerCamera.transform.position;
            if (enemy != null)
            {
                enemy.TakeDamage(damage, pushDirection);
            }

            // Optionally add some attack effects or animations here
        }
    }
}

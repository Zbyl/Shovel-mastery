using System;
using UnityEngine;

public class Shovel : MonoBehaviour
{
    public float damage = 10.0f;      // Damage dealt by the pickaxe
    public float attackRange = 7.0f;  // Range of the pickaxe attack
    public float attackRate = 10.1f;   // per 1 second
    public Camera playerCamera;       // Reference to the player's camera

    private float nextAttackTime = 0.0f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

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
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * attackRange, Color.red, 3.0f);
        bool raycastHit = Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward * attackRange, out hit, attackRange);
        if (raycastHit)
        {
            if (hit.transform.CompareTag("Zombie"))
            {
                Zombie enemy = hit.transform.GetComponent<Zombie>();
                Debug.Log($"Hit: {hit.transform.name}");
                animator.SetTrigger("Attack");
                if (enemy != null)
                {
                    enemy.TakeHit(damage, hit.point - playerCamera.transform.position);
                }
            }
            else
            {
                animator.SetTrigger("AttackMiss");
            }

        }
        else
        {
            animator.SetTrigger("AttackMiss");
        }
    }
}

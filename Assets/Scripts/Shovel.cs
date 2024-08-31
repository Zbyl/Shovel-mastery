using System;
using UnityEngine;

using System.Collections;

public class Shovel : MonoBehaviour
{
    public float damage = 10.0f;      // Damage dealt by the pickaxe
    public float attackRange = 7.0f;  // Range of the pickaxe attack
    public Camera playerCamera;       // Reference to the player's camera

    public float preAttackDelay = 0.3f; // Time in seconds to wait before the attack happens
    public float attackCooldown = 0.0f; // Time in seconds before the player can attack again
    private bool canAttack = true;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && canAttack)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        canAttack = false; // Prevent multiple attacks during the delay

        // Optional: Play a pre-attack animation or sound here
        RaycastHit hit;
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * attackRange, Color.red, 3.0f);
        bool raycastHit = Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward * attackRange, out hit, attackRange);

        if (raycastHit)
        {
            if (hit.transform.CompareTag("Zombie"))
            {
                animator.SetTrigger("Attack");
                Zombie enemy = hit.transform.GetComponent<Zombie>();
                Debug.Log($"Hit: {hit.transform.name} {hit.transform.GetHashCode()}");
                yield return new WaitForSeconds(preAttackDelay);
                enemy.TakeHit(damage, hit.point, hit.point - playerCamera.transform.position);
            }
            else if (hit.transform.CompareTag("Grave"))
            {
                animator.SetTrigger("Attack");
                Grave grave = hit.transform.GetComponent<Grave>();
                Debug.Log($"Hit: {hit.transform.name} {hit.transform.GetHashCode()}");
                yield return new WaitForSeconds(preAttackDelay);
                grave.Dig();
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
        yield return new WaitForSeconds(attackCooldown);

        canAttack = true; // Allow the player to attack again
    }

}

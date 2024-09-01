using System;
using UnityEngine;

using System.Collections;

public class Shovel : MonoBehaviour
{
    public float damage = 10.0f;      // Damage dealt by the pickaxe
    public float attackRange = 4.0f;  // Range of the pickaxe attack
    public Camera playerCamera;       // Reference to the player's camera
    public WavePushback wavePrefab;     // Prefab for the wave effect

    public float preAttackDelay = 0.3f; // Time in seconds to wait before the attack happens
    public float attackCooldown = 0.5f; // Time in seconds before the player can attack again
    private bool canAttack = true;

    private Animator animator;

    private AudioSource missSound;
    private AudioSource dirtSound;
    private AudioSource stoneSound;
    private AudioSource waveSound;

    public GameObject swingParticlesPrefab; // Particles for swinging shovel, hitting not-stone and not metal.
    public GameObject stoneParticlesPrefab; // Particles for swinging shovel, hitting stone and metal.

    void Start()
    {
        missSound = transform.Find("MissSound").GetComponent<AudioSource>();
        dirtSound = transform.Find("DirtSound").GetComponent<AudioSource>();
        stoneSound = transform.Find("StoneSound").GetComponent<AudioSource>();
        waveSound = transform.Find("WaveSound").GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (GameState.Instance.isPaused) return;

        if (Input.GetButtonDown("Fire1") && canAttack)
        {
            StartCoroutine(Attack());
        }
        else if (Input.GetButtonDown("Fire2") && canAttack)
        {
            StartCoroutine(Wave());
        }
    }

    IEnumerator Wave()
    {
        waveSound.Play();
        canAttack = false;
        animator.SetTrigger("AttackMiss");
        yield return new WaitForSeconds(preAttackDelay);
        Instantiate(wavePrefab, transform.transform.forward * 4, Quaternion.identity);
        yield return new WaitForSeconds(preAttackDelay);
        canAttack = true;
    }

    IEnumerator Attack()
    {
        canAttack = false;

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
                //ShowSwingParticles(false);
                enemy.TakeHit(damage, hit.point, hit.point - playerCamera.transform.position, false);
            }
            else if (hit.transform.CompareTag("Grave"))
            {
                animator.SetTrigger("Attack");
                Grave grave = hit.transform.GetComponent<Grave>();
                Debug.Log($"Hit: {hit.transform.name} {hit.transform.GetHashCode()}");
                yield return new WaitForSeconds(preAttackDelay);
                ShowSwingParticles(false);
                grave.Dig();
                dirtSound.Play();
            }
            else if (hit.transform.CompareTag("Ground"))
            {
                animator.SetTrigger("Attack");
                yield return new WaitForSeconds(preAttackDelay);
                ShowSwingParticles(false);
                dirtSound.Play();

            }
            else if (hit.transform.CompareTag("Stone"))
            {
                animator.SetTrigger("Attack");
                yield return new WaitForSeconds(preAttackDelay);
                ShowSwingParticles(true);
                stoneSound.Play();
            }
            else
            {
                animator.SetTrigger("AttackMiss");
                missSound.Play();
            }
        }
        else
        {
            animator.SetTrigger("AttackMiss");
            missSound.Play();
        }
        yield return new WaitForSeconds(attackCooldown);

        canAttack = true; // Allow the player to attack again
    }

    void ShowSwingParticles(bool stone)
    {
        var q = new Quaternion();
        var hitParticles = Instantiate(stone ? stoneParticlesPrefab : swingParticlesPrefab, transform.parent.transform.position + transform.parent.transform.forward * 0.5f, q, transform.parent);
    }
}

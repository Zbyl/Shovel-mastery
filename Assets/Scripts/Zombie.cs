using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    private Transform target;
    private NavMeshAgent navMeshAgent;

    private Vector3 currentPushForce = Vector3.zero;   /// Force used to simulate Zombie being pushed.
    public float pushForceDecay = 0.1f;              /// How quickly pushForce goes down to zero. <summary>

    public float health = 500.0f;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        navMeshAgent.destination = target.position;
        transform.position += currentPushForce;
        currentPushForce = Vector3.MoveTowards(currentPushForce, Vector3.zero, pushForceDecay);
    }

    public void TakeDamage(float damage, Vector3 pushDirection)
    {
        health -= damage;
        Debug.Log($"Damage taken: {damage}");

        float pushForce = 0.5f;

        Debug.Log($"tempCounter is: {GameState.Instance.tempCounter}");
        currentPushForce = pushDirection.normalized * pushForce;


        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Add death effects, animations, etc.
        Destroy(gameObject);
    }
}

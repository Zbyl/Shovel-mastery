using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent navMeshAgent;

    private Vector3 currentPushForce = Vector3.zero;   /// Force used to simulate Zombie being pushed.
    public float pushForceDecay = 0.1f;              /// How quickly pushForce goes down to zero. <summary>

    public float health = 50.0f;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        navMeshAgent.destination = target.position;

        /// Temporary code for pushing Zombies randomly.
        var result = Random.Range(10, 100);
        if (result < 1)
        {
            WasHit(new Vector3(1.0f, 0.0f, 0.0f));
        }

        transform.position += currentPushForce;
        currentPushForce = Vector3.MoveTowards(currentPushForce, Vector3.zero, pushForceDecay);

    }

    /// Call this function when Zombie was hit by the player.
    /// It will push the Zombie backwards.
    void WasHit(Vector3 pushDirection, float pushForce = 1.0f)
    {
        Debug.Log($"tempCounter is: {GameState.Instance.tempCounter}");
        currentPushForce = pushDirection.normalized * pushForce;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"Damage taken: {damage}");

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

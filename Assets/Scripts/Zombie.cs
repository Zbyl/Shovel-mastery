using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    private Transform target;
    float minimalDistanceToTarget = 2.0f;      /// We won't come closer to target than this.
    public float minimalDistanceToTargetDefault = 2.0f;      /// We won't come closer to target than this.
    public float minimalDistanceToTargetForCowards = 3.0f;      /// Cowardly Zombies won't come closer to target than this.
    public float cowardProbability = 0.5f;      /// How likely it is to have a cowardly Zombie. It will have effecting minimalDistanceToTarget a bit random.
    private NavMeshAgent navMeshAgent;

    private Vector3 currentPushForce = Vector3.zero;   /// Force used to simulate Zombie being pushed.
    public float pushForceDecay = 0.01f;              /// How quickly pushForce goes down to zero.
    public float pushForce = 0.7f;                    /// Strength of a push.

    public float health = 500.0f;

    public float stunTime = 3.0f;
    private float stunTimer = 0.0f;
    private bool isStunned = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        Transform player_target = GameObject.Find("Player").transform;
        target = player_target;
        if (Random.Range(0.0f, 1.0f) <= cowardProbability)
        {
            minimalDistanceToTarget = Random.Range(minimalDistanceToTargetDefault, minimalDistanceToTargetForCowards);
        } else {
            minimalDistanceToTarget = minimalDistanceToTargetDefault;
        }
    }

    void FixedUpdate()
    {
        var vecToDestination = target.position - transform.position;
        var targetPoint = target.position - vecToDestination.normalized * minimalDistanceToTarget;
        navMeshAgent.destination = targetPoint;
        navMeshAgent.isStopped = isStunned;

        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                isStunned = false;
            }
        }
        transform.position += currentPushForce;
        currentPushForce = Vector3.MoveTowards(currentPushForce, Vector3.zero, pushForceDecay);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            collision.gameObject.GetComponent<Zombie>().TakeHit(0.0f, collision.contacts[0].point - transform.position);
        }
    }

    public void TakeHit(float damage, Vector3 pushDirection)
    {
        if (!isStunned)
        {
            isStunned = true;
            stunTimer = stunTime;
            Debug.Log("Zombie is stunned");
        }
        else
        {
            health -= damage;
            Debug.Log($"Damage taken: {damage}");
            currentPushForce = pushDirection.normalized * pushForce;
        }

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

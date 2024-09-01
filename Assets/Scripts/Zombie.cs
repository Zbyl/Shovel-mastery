using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Zombie : MonoBehaviour
{
    private Transform target;
    private FPSController player;

    GameState game;
    float minimalDistanceToTarget = 2.0f;      /// We won't come closer to target than this.
    public float minimalDistanceToTargetDefault = 2.0f;      /// We won't come closer to target than this.
    public float minimalDistanceToTargetForCowards = 3.0f;      /// Cowardly Zombies won't come closer to target than this.
    public float cowardProbability = 0.5f;      /// How likely it is to have a cowardly Zombie. It will have effecting minimalDistanceToTarget a bit random.
    private NavMeshAgent navMeshAgent;

    private Vector3 currentPushForce = Vector3.zero;   /// Force used to simulate Zombie being pushed.
    public float pushForceDecay = 0.01f;              /// How quickly pushForce goes down to zero.
    public float pushForce = 0.7f;                    /// Strength of a push.

    public float health = 50.0f;

    public float stunTime = 3.0f;
    private float stunTimer = 0.0f;
    private bool isStunned = false;

    public Transform animatedMesh;  /// Must be set to the mesh with Animator component.
    private Animator animator;

    private AudioSource footsteps;
    private AudioSource waveSound;
    private AudioSource[] painSounds;

    public GameObject hitParticlesPrefab; // Prefab for hit particles.
    public GameObject skeletonBonesPrefab; // Prefab for bones.

    // Zombie AI
    // If Zombie is within superCloseRadius it walks straight to the player.
    // If Zombie is within farRadius it walks to a random position on closeRadius, within approachAngle.
    // Otherwise zombie walks in random direction for a random distance within randomRadius.
    private float superCloseRadius = 7.0f;
    private float attackRadius = 4.0f;
    private float closeRadius = 6.0f;
    private float farRadius = 20.0f;
    private float randomRadius = 10.0f;
    private float approachAngle = 75.0f; // In degrees.
    private bool isDead = false;

    private bool showDebugTarget = false;
    public GameObject debugTargetPrefab;
    private GameObject debugTarget;

    private float hitDelay = 5f;

    enum ZombieState
    {
        RandomWalk,
        Approach,
        Direct,
    }
    private ZombieState zombieState = ZombieState.Direct;
    private Vector3 zombieTarget = Vector3.zero;
    void ZombieAI()
    {
        var dirToPlayer = (target.position - transform.position).normalized;
        var distToPlayer = (target.position - transform.position).magnitude;
        var dirToCurrentTarget = (zombieTarget - transform.position).normalized;
        var distToCurrentTarget = (zombieTarget - transform.position).magnitude;

        var desiredState = ZombieState.RandomWalk;
        if (distToPlayer < farRadius)
        {
            desiredState = ZombieState.Approach;
        }
        if (distToPlayer < superCloseRadius)
        {
            desiredState = ZombieState.Direct;
        }

        var lookForNewTarget = false;
        if ((zombieState == desiredState) && (distToCurrentTarget < 1.0f))
        {
            //Debug.Log($"Looking for new target for {zombieState}");
            lookForNewTarget = true;
        }

        if ((desiredState != zombieState) || lookForNewTarget)
        {
            if (desiredState == ZombieState.RandomWalk)
            {
                var angle = Random.Range(0.0f, 360.0f);
                var offset = Quaternion.Euler(0.0f, angle, 0.0f) * Vector3.forward * randomRadius;
                zombieTarget = transform.position + offset;
            }
            if (desiredState == ZombieState.Approach)
            {
                var angle = Random.Range(-approachAngle, approachAngle);
                var offset = Quaternion.Euler(0.0f, angle, 0.0f) * -dirToPlayer * closeRadius;
                zombieTarget = target.position + offset;
            }

            NavMeshHit hit;
            if (NavMesh.SamplePosition(zombieTarget, out hit, 100.0f, NavMesh.AllAreas))
            {
                zombieTarget = hit.position;
            }
            else
            {
                //Debug.Log($"Cannot find target on navmesh.");
            }

            //Debug.Log($"Switching from {zombieState} to {desiredState}. Target: {zombieTarget}");
            zombieState = desiredState;
        }

        if (zombieState == ZombieState.Direct)
        {
            zombieTarget = target.position - dirToPlayer.normalized * minimalDistanceToTarget;
        }

        if (showDebugTarget)
        {
            debugTarget.transform.position = zombieTarget;
        }
    }

    void Awake()
    {
        footsteps = transform.Find("Footsteps").GetComponent<AudioSource>();
        waveSound = transform.Find("WaveSound").GetComponent<AudioSource>();
        painSounds = transform.Find("PainSounds").GetComponentsInChildren<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.Find("GameState").GetComponent<GameState>();
        animator = animatedMesh.GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        Transform player_target = GameObject.Find("Player").transform;
        player = player_target.GetComponent<FPSController>();
        target = player_target;
        if (Random.Range(0.0f, 1.0f) <= cowardProbability)
        {
            minimalDistanceToTarget = Random.Range(minimalDistanceToTargetDefault, minimalDistanceToTargetForCowards);
        }
        else
        {
            minimalDistanceToTarget = minimalDistanceToTargetDefault;
        }

        if (showDebugTarget)
        {
            debugTarget = Instantiate(debugTargetPrefab, transform.position, transform.rotation, transform.parent);
        }
    }

    void FixedUpdate()
    {
        if (GameState.Instance.isPaused)
        {
            navMeshAgent.isStopped = true;
            return;
        }

        ZombieAI();

        if (hitDelay > 0)
        {
            hitDelay -= Time.deltaTime;
        }

        navMeshAgent.destination = zombieTarget;
        navMeshAgent.isStopped = isStunned;

        var wasWalking = animator.GetBool("Walking");
        var isWalking = (zombieTarget - transform.position).magnitude > 0.1f;
        animator.SetBool("Walking", isWalking);
        if (wasWalking != isWalking)
        {
            //Debug.Log($"Walking: {isWalking}");
            if (isWalking)
            {
                footsteps.Play();
                footsteps.time = Random.Range(0.0f, footsteps.clip.length);
            }
            else
            {
                footsteps.Stop();
            }
        }

        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                isStunned = false;
            }
        }
        animator.SetBool("Stunned", isStunned);

        float distToPlayer = (target.position - transform.position).magnitude;
        if (distToPlayer < attackRadius && hitDelay <= 0 && !isStunned && !isDead && !isWalking && !wasWalking)
        {
            StartCoroutine(Attack());
        }

        transform.position += currentPushForce;
        currentPushForce = Vector3.MoveTowards(currentPushForce, Vector3.zero, pushForceDecay);

        GameState.Instance.ZombieSoundTick(this);
    }

    IEnumerator Attack()
    {
        hitDelay = 5f;
        float distToPlayer = (target.position - transform.position).magnitude;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(3.0f);
        
        Debug.Log($"Zombie {this.GetHashCode()} old rad {distToPlayer}");
        distToPlayer = (target.position - transform.position).magnitude;
        Debug.Log($"Zombie {this.GetHashCode()} new rad {distToPlayer}");
        if (distToPlayer < attackRadius)
        {
            Debug.Log($"Zombie {this.GetHashCode()} and hits");
            game.TakeHit(1);
        }
        else
        {
            Debug.Log($"Zombie {this.GetHashCode()} missed");
        }
    }



    void OnCollisionEnter(Collision other)
    {
        Debug.Log($"Collision with {other.gameObject.tag}");
        if (other.gameObject.CompareTag("Zombie"))
        {
            var collisionPoint = other.contacts[0].point;
            other.gameObject.GetComponent<Zombie>().TakeHit(0.0f, collisionPoint, collisionPoint - transform.position, false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger with {other.gameObject.tag}");
        if (other.gameObject.CompareTag("DieArea"))
        {
            Grave grave = other.gameObject.GetComponentInParent<Grave>();
            if (grave.isDeadly)
            {
                Debug.Log($"Zombie {this.GetHashCode()} died in a grave");
                grave.CloseGrave();
                Die();
            }
        }
    }


    public void TakeHit(float damage, Vector3 pushPosition, Vector3 pushDirection, bool waveHit)
    {
        if (isDead) return; /// Attempt to fix bug with null sound. Probably won't work...

        var q = new Quaternion();
        q.SetFromToRotation(Vector3.up, -pushDirection.normalized);
        var hitParticles = Instantiate(hitParticlesPrefab, pushPosition, q);

        if (waveHit)
        {
            waveSound.Play();
        }

        if (!isStunned)
        {
            isStunned = true;
            stunTimer = stunTime;
            Debug.Log($"Zombie {this.GetHashCode()} is stunned");
            animator.SetTrigger("StunTrigger");
        }

        if (damage > 0)
        {
            health -= damage;
            Debug.Log($"Damage taken: {damage} {this.GetHashCode()}");
            painSounds[Random.Range(0, painSounds.Length)].Play();
        }

        currentPushForce = pushDirection.normalized * pushForce;
        animator.SetTrigger("Hit");

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        GameState.Instance.skeletonsKilled += 1;
        // Add death effects, animations, etc.
        var bones = Instantiate(skeletonBonesPrefab, transform.position, transform.rotation, transform.parent);
        Destroy(gameObject);
    }
}

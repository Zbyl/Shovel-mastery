using UnityEngine;

public class WavePushback : MonoBehaviour
{
    public float waveRadius = 5.0f;        // Radius of the wave effect
    public float pushForce = 10.0f;        // Force applied to enemies
    public LayerMask enemyLayer;           // LayerMask to identify enemies

    void Start()
    {
        Collider[] enemiesHit = Physics.OverlapSphere(transform.position, waveRadius, enemyLayer);
        foreach (Collider enemy in enemiesHit)
        {
            enemy.GetComponent<Zombie>().TakeHit(0, enemy.transform.position, enemy.transform.position - transform.position);
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, waveRadius);
    }
}

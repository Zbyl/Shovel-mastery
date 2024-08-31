using UnityEngine;

public class WavePushback : MonoBehaviour
{
    public float waveRadius = 5.0f;        // Radius of the wave effect
    public float pushForce = 10.0f;        // Force applied to enemies
    public float waveHitForce = 10.0f;     // Force applied to enemies hit by the wave
    public LayerMask enemyLayer;           // LayerMask to identify enemies


    void Start()
    {
        Collider[] enemiesHit = Physics.OverlapSphere(transform.position, waveRadius, enemyLayer);
        foreach (Collider enemy in enemiesHit)
        {
            enemy.GetComponent<Zombie>().TakeHit(waveHitForce, enemy.transform.position, enemy.transform.position - transform.position, true);
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, waveRadius);
    }
}

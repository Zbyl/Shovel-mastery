using UnityEngine;
using System.Collections; // Add this line to import the IEnumerator type

public class WavePushback : MonoBehaviour
{
    public float waveRadius = 5.0f;        // Radius of the wave effect
    public float pushForce = 10.0f;        // Force applied to enemies
    public float waveHitForce = 15.0f;     // Force applied to enemies hit by the wave
    public float waveHitForceMultiplayer = 0.0f;
    public LayerMask enemyLayer;           // LayerMask to identify enemies

    public Vector3 direction;
    void Start()
    {
        StartCoroutine(DelayedDestroy());
    }

    void Update()
    {
        transform.position += direction * Time.deltaTime * 10;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Vector3 pushDirection = other.transform.position - transform.position;
            pushDirection.y = 0;
            other.GetComponent<Zombie>().TakeHit(waveHitForceMultiplayer * waveHitForce, other.transform.position, pushDirection, false);
        }
    }

    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, waveRadius);
    }
}

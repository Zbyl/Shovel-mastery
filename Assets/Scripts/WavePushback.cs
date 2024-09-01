using UnityEngine;
using System.Collections; // Add this line to import the IEnumerator type

public class WavePushback : MonoBehaviour
{
    public float waveRadius = 5.0f;        // Radius of the wave effect
    public float pushForce = 10.0f;        // Force applied to enemies
    public float waveHitForce = 10.0f;     // Force applied to enemies hit by the wave
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
            other.GetComponent<Zombie>().TakeHit(pushForce, other.transform.position, pushDirection, false);
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

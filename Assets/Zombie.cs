using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent navMeshAgent;
    private Vector3 pushForce = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        navMeshAgent.destination = target.position;
        var result = Random.Range(0, 100);
        if (result < 1) {
            pushForce = new Vector3(1.0f, 0.0f, 0.0f);
        }
        transform.position += pushForce;
        pushForce = Vector3.MoveTowards(pushForce, Vector3.zero, 0.1f);
    }
}

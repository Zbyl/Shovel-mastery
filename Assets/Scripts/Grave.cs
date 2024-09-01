using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class Grave : MonoBehaviour
{
    // Start is called before the first frame update
    public float graveRespawnTime = 10.0f;
    public float graveMaxRespawnTime = 10.0f;
    public float graveDigTime = 2.0f;

    public float invincibilityTime = 15.0f;
    public float maxInvincibilityTime = 15.0f;

    public Transform spawnPoint;
    public Collider dieArea;

    public bool grave_opened = false;
    public bool grave_sealed = false;
    public bool isDeadly = false;

    private ParticleSystem vfx;

    void Start()
    {
        spawnPoint = transform.Find("SpawnPoint");
        dieArea = transform.Find("DieArea").GetComponent<Collider>();
        vfx = transform.Find("vfx_grave").GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (invincibilityTime > 0 && grave_opened)
        {
            invincibilityTime -= Time.deltaTime;
        }

        if (invincibilityTime <= 0 && !isDeadly)
        {
            isDeadly = true;
            dieArea.enabled = true;
            Debug.Log("Grave is deadly");
        }
    }

    public void Dig()
    {
        Debug.Log("Grave digged");
        graveRespawnTime += Math.Min(graveDigTime, graveMaxRespawnTime);
    }

    public void OpenGrave()
    {
        StartCoroutine(SetGraveLid(false));
        isDeadly = false;
        invincibilityTime = maxInvincibilityTime;
        grave_opened = true;
        grave_sealed = true;
    }

    public void CloseGrave()
    {
        StartCoroutine(SetGraveLid(false)); // on purpose
        grave_opened = false;
        grave_sealed = true;
    }

    IEnumerator SetGraveLid(bool enabled)
    {
        Transform childTransform = transform.Find("grave");

        if (childTransform != null)
        {
            MeshRenderer mesh = childTransform.GetComponent<MeshRenderer>();
            MeshCollider collider = childTransform.GetComponent<MeshCollider>();
            NavMeshObstacle navMeshObstacle = childTransform.GetComponent<NavMeshObstacle>();

            if (mesh.enabled != enabled || collider.enabled != enabled || navMeshObstacle.enabled != enabled)
            {
                vfx.Play();
                yield return new WaitForSeconds(0.3f);
            }

            collider.enabled = enabled;
            mesh.enabled = enabled;
            navMeshObstacle.enabled = enabled;
        }
    }
}

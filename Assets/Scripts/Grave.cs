using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Grave : MonoBehaviour
{
    // Start is called before the first frame update
    public float graveRespawnTime = 10.0f;
    public float graveMaxRespawnTime = 10.0f;
    public float graveDigTime = 2.0f;

    public float invincibilityTime = 15.0f;

    public Transform spawnPoint;
    public Collider dieArea;

    public bool grave_opened = false;
    public bool grave_sealed = false;
    public bool isDeadly = false;

    void Start()
    {
        spawnPoint = transform.Find("SpawnPoint");
        dieArea = transform.Find("DieArea").GetComponent<Collider>();
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
        grave_opened = true;
        SetGraveLid(false);
    }

    public void CloseGrave()
    {
        grave_opened = false;
        grave_sealed = true;
    }

    private void SetGraveLid(bool enabled)
    {
        Transform childTransform = transform.Find("grave");

        if (childTransform != null)
        {
            MeshRenderer mesh = childTransform.GetComponent<MeshRenderer>();
            mesh.enabled = enabled;
            MeshCollider collider = childTransform.GetComponent<MeshCollider>();
            collider.enabled = enabled;
        }
    }
}

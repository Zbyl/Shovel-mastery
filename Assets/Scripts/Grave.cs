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

    public float invincibilityTime = 10.0f;

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
        if (!isDeadly && grave_opened){
            invincibilityTime -= Time.deltaTime;
        }else{
            isDeadly = true;
        }
    }

    public void Dig()
    {
        Debug.Log("Grave digged");
        graveRespawnTime += Math.Min(graveDigTime, graveMaxRespawnTime);
    }

    public void OpenGrave()
    {
        if (GameState.Instance.isPaused) return;
        grave_opened = true;
        SetGraveLid(false);
        dieArea.enabled = false;
    }

    public void CloseGrave()
    {
        grave_opened = false;
        grave_sealed = true;
        SetGraveLid(true);
    }

    private void SetGraveLid(bool open)
    {
        Transform childTransform = transform.Find("grave");

        if (childTransform != null)
        {
            MeshRenderer mesh = childTransform.GetComponent<MeshRenderer>();
            mesh.enabled = false;
            MeshCollider collider = childTransform.GetComponent<MeshCollider>();
            collider.enabled = false;
        }
    }
}

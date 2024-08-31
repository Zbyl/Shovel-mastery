using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Grave : MonoBehaviour
{
    // Start is called before the first frame update
    public float graveRespawnTime = 10.0f;
    public float graveSpawnDelay = 10.0f;
    public float graveMaxSpawnDelay = 10.0f;
    public float graveMaxRespawnTime = 10.0f;
    public float graveDigTime = 2.0f;

    public bool grave_opened = false;

    public void Dig()
    {
        Debug.Log("Grave digged");
        graveRespawnTime += Math.Min(graveDigTime, graveMaxRespawnTime);
    }

    public void OpenGrave()
    {
        // Find the child GameObject by name
        Transform childTransform = transform.Find("grave");
        grave_opened = true;
        
        if (childTransform != null)
        {
            // Get the component from the child GameObject
            MeshRenderer mesh = childTransform.GetComponent<MeshRenderer>();
            mesh.enabled = false;
            MeshCollider collider = childTransform.GetComponent<MeshCollider>();
            collider.enabled = false;

            Debug.Log("Grave opened.");
        }

    }
}
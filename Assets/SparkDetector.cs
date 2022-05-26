using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkDetector : MonoBehaviour
{
    public ParticleSystem sparks;
    
    private void OnTriggerEnter(Collider other)
    {
        var sparksEmission = sparks.emission;
        sparksEmission.enabled = true;
        Debug.Log("ENTERED");

    }

    private void OnTriggerExit(Collider other)
    {
        var sparksEmission = sparks.emission;
        sparksEmission.enabled = false;
        Debug.Log("LEFT");
    }
}

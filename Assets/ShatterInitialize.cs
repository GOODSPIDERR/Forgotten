using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShatterInitialize : MonoBehaviour
{
    private float _shatterTimer = 5f;
    private Rigidbody[] _rbs;
    private MeshCollider[] _colliders;
    private ShatterInitialize selfScript;
    public Transform _playerTransform;
    void Awake()
    {
        _rbs = GetComponentsInChildren<Rigidbody>();
        _colliders = GetComponentsInChildren<MeshCollider>();
        selfScript = GetComponent<ShatterInitialize>();
    }

    private void Start()
    {
        StartCoroutine(nameof(TimedDeletion), _shatterTimer);
        bool boolean = Random.value > 0.5f;
        InitialForce(boolean);
    }
    
    IEnumerator TimedDeletion(float shatterTimer)
    {
        yield return new WaitForSeconds(shatterTimer);
        
        foreach (var rb in _rbs)
        {
            Destroy(rb);
        }

        foreach (var meshCollider in _colliders)
        {
            Destroy(meshCollider);
        }
        
        Destroy(selfScript);
    }

    private void InitialForce(bool right) //This command is fucking wonky. Needs revisiting later
    {
        foreach (var rb in _rbs)
        {
            if (right)  //Idk why this doesn't work the way I want it to :/
                rb.AddForce((transform.position - _playerTransform.position).normalized * 5, ForceMode.Impulse);
            else
            {
                rb.AddForce((transform.position - _playerTransform.position).normalized * 5, ForceMode.Impulse);
            }
        }

    }
}

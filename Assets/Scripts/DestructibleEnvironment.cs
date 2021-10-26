using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DestructibleEnvironment : MonoBehaviour
{
    public int health = 3;
    private Vector3 _initialPosition;
    public GameObject shatterObject;

    private void Start()
    {
        _initialPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Attack")) return;
        transform.position = _initialPosition;
        health--;
        transform.DOShakePosition(0.5f, 0.2f, 50, 90, false, true).OnComplete(() => { transform.DOMove(_initialPosition, 0.25f); });

        if (health <= 0)
        {
            Instantiate(shatterObject, transform.position, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
            Destroy(gameObject);
        }
    }
}

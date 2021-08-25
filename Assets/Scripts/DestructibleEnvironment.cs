using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DestructibleEnvironment : MonoBehaviour
{
    public int health = 3;
    Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Attack"))
        {
            transform.position = initialPosition;
            health--;
            transform.DOShakePosition(0.5f, 0.2f, 50, 90, false, true).OnComplete(() => { transform.DOMove(initialPosition, 0.25f); });

            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}

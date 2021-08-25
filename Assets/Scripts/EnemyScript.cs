using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int health = 1;
    public GameObject bloodSplatter;
    GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attack"))
        {
            health--;
            if (health <= 0)
            {
                Vector3 difference = transform.position - player.transform.position;

                float targetAngle = Mathf.Atan2(difference.x, difference.z) * Mathf.Rad2Deg;
                Instantiate(bloodSplatter, transform.position, Quaternion.Euler(0, targetAngle, 0));
                //Destroy(gameObject);
            }
        }
    }
}
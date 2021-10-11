using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyTest : MonoBehaviour
{

    GameObject player;
    NavMeshAgent navMesh;
    PlayerHealth playerHealth;
    float invincibility = 0f;
    float originalSpeed;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        navMesh = GetComponent<NavMeshAgent>();
        originalSpeed = navMesh.speed;
    }

    void Update()
    {
        //Invincibility frames are on the enemy rather than on the player. This means that the player can get hit many times in quick succession, but only from different enemies. 
        invincibility -= Time.deltaTime;
        Vector3 trueDistance = player.transform.position - transform.position;
        Vector2 distance = new Vector2(trueDistance.x, trueDistance.z);

        //After hitting the player, slowly regain movement speed. 
        navMesh.speed += Time.deltaTime;
        navMesh.speed = Mathf.Clamp(navMesh.speed, 0f, originalSpeed);
        navMesh.destination = player.transform.position;

        if ((Mathf.Abs(distance.magnitude) <= 1f) && invincibility <= 0f)
        {
            playerHealth.TakeDamage(gameObject);
            invincibility = 1f;
            navMesh.speed = 0f;
        }
    }
}

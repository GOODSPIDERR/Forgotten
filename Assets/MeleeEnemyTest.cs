using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyTest : MonoBehaviour
{

    GameObject player;
    NavMeshAgent navMesh;
    PlayerAttack playerAttack;
    float invincibility = 0f;
    float originalSpeed;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerAttack = player.GetComponent<PlayerAttack>();
        navMesh = GetComponent<NavMeshAgent>();
        originalSpeed = navMesh.speed;

    }

    // Update is called once per frame
    void Update()
    {
        invincibility -= Time.deltaTime;
        Vector3 trueDistance = player.transform.position - transform.position;
        Vector2 distance = new Vector2(trueDistance.x, trueDistance.z);
        //Debug.Log(distance);
        //Debug.Log(invincibility);

        navMesh.speed += Time.deltaTime;
        navMesh.speed = Mathf.Clamp(navMesh.speed, 0f, originalSpeed);

        navMesh.destination = player.transform.position;

        if ((Mathf.Abs(distance.magnitude) <= 1f) && invincibility <= 0f)
        {
            playerAttack.TakeDamage(gameObject);
            invincibility = 1f;
            navMesh.speed = 0f;
        }
    }
}

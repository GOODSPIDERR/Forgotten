using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyTest : MonoBehaviour
{
    private GameObject _player;
    private NavMeshAgent _navMesh;
    private PlayerHealth _playerHealth;
    private float _invincibility = 0f;
    private float _originalSpeed;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerHealth = _player.GetComponent<PlayerHealth>();
        _navMesh = GetComponent<NavMeshAgent>();
        _originalSpeed = _navMesh.speed;
    }

    private void Update()
    {
        //Invincibility frames are on the enemy rather than on the player. This means that the player can get hit many times in quick succession, but only from different enemies. 
        _invincibility -= Time.deltaTime;
        var position = _player.transform.position;
        var trueDistance = position - transform.position;
        var distance = new Vector2(trueDistance.x, trueDistance.z);

        //After hitting the player, slowly regain movement speed. 
        var speed = _navMesh.speed;
        speed += Time.deltaTime;
        _navMesh.speed = speed;
        _navMesh.speed = Mathf.Clamp(speed, 0f, _originalSpeed);
        _navMesh.destination = position;

        if ((!(Mathf.Abs(distance.magnitude) <= 1f)) || !(_invincibility <= 0f)) return;
        _playerHealth.TakeDamage(gameObject);
        _invincibility = 1f;
        _navMesh.speed = 0f;
    }
}

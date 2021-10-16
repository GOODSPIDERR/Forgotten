using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int health = 1;
    public GameObject bloodSplatter;
    private GameObject _player;
    public Material bloodMaterial;
    public PlayerHealth playerHealth;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Attack")) return;
        health--;
        if (health > 0) return;
        playerHealth.IncreaseBlood(0.25f);
        var position = transform.position;
        var difference = _player.transform.position - position;

        var targetAngle = Mathf.Atan2(difference.x, difference.z) * Mathf.Rad2Deg;
        Instantiate(bloodSplatter, position, Quaternion.Euler(0, targetAngle, 0));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.VFX;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Cinemachine;

public class PlayerAttack : MonoBehaviour
{
    PlayerMovement playerMovement;
    PlayerHealth playerHealth;

    public int stance = 0;
    public GameObject fastSwing1, fastSwing2;
    public float rotationTimer = 0f;
    float targetAngle = 0f;
    public bool canAttack;
    public float cooldown;
    float cooldownMeter;

    PlayerInput playerInput;
    PlayerInputActions playerInputActions;
    Animator animator;
    [HideInInspector] public Transform enemyThatCould;

    public CameraShake cameraShake;

    //This script really needs to be broken up into 2 scripts: 1 for attacking and 1 for hp

    void Start()
    {
        #region Getters & Setters
        //Link
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();

        //Input system
        playerInput = GetComponent<PlayerInput>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        animator = GetComponent<Animator>();
        #endregion
    }
    void Update()
    {


        Vector2 attackVector = playerInputActions.Player.Attack.ReadValue<Vector2>();

        //Debug.Log(attackVector);

        float xAttack = attackVector.x;
        float yAttack = attackVector.y;

        Vector3 swingDirection = new Vector3(xAttack, 0, yAttack);

        cooldownMeter -= Time.deltaTime;

        animator.SetFloat("RotationTimer", rotationTimer);

        //if (cooldownMeter <= 0f) canAttack = true; else canAttack = false; //Maybe tying the canAttack directly to the cooldown isn't the best idea

        if (swingDirection.magnitude > 0.1f)
        {
            if (canAttack) targetAngle = Mathf.Atan2(swingDirection.x, swingDirection.z) * Mathf.Rad2Deg;
            if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) && canAttack && cooldownMeter <= 0f)
            {
                cameraShake.Shake(0.8f, 1f, 0.25f);
                
                //This needs to be re-written
                switch (stance)
                {
                    case 0:
                        stance = 1;
                        rotationTimer = 0.75f;
                        transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
                        animator.SetTrigger("Slash");
                        Attack(0);
                        break;
                    case 1:
                        stance = 2;
                        rotationTimer = 0.75f;
                        transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
                        animator.SetTrigger("Slash");
                        Attack(1);
                        break;
                    case 2:
                        stance = 1;
                        rotationTimer = 0.75f;
                        transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
                        animator.SetTrigger("Slash");
                        Attack(2);
                        break;
                }
            }
        }

        if (rotationTimer > 0f && stance != 0)
        {
            rotationTimer -= 1f * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
        }
        else
        {
            stance = 0;
        }

        //This is such fucking spaghetti code lmao. Refactor this later
        void Attack(int stance)
        {
            cooldownMeter = cooldown;
            GameObject targetSwing = fastSwing1;
            switch (stance)
            {
                case 0:
                    targetSwing = fastSwing1;
                    break;
                case 1:
                    targetSwing = fastSwing2;
                    break;
                case 2:
                    targetSwing = fastSwing1;
                    break;
            }

            var swing = Instantiate(targetSwing, transform.position + transform.forward + transform.up, transform.rotation);
            swing.transform.parent = gameObject.transform;
        }
    }

    public void SetAttack(bool attack)
    {
        canAttack = attack;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.VFX;

public class PlayerAttack : MonoBehaviour
{
    public int stance = 0;
    public GameObject fastSwing1, fastSwing2;
    public float rotationTimer = 0f;
    float targetAngle = 0f;
    public bool canAttack;
    public float blood = 0f;
    public float cooldown;
    float cooldownMeter;

    public Material bloodMaterial;
    PlayerInput playerInput;
    PlayerInputActions playerInputActions;
    public Slider slider;
    public VisualEffect bloodDrip;
    bool playBool = false;

    void Start()
    {
        canAttack = true;
        playerInput = GetComponent<PlayerInput>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }
    void Update()
    {
        float bloodDrain;
        bloodDrain = blood < 1f ? 0.25f : 0.5f;
        blood -= bloodDrain * Time.deltaTime;
        blood = Mathf.Clamp(blood, 0f, 1.5f);
        slider.value = blood;

        if (blood > 0) bloodDrip.enabled = true;
        else bloodDrip.enabled = false;

        bloodMaterial.SetFloat("Cleanliness_", 1f - blood);

        Vector2 attackVector = playerInputActions.Player.Attack.ReadValue<Vector2>();

        Debug.Log(attackVector);

        float xAttack = attackVector.x;
        float yAttack = attackVector.y;

        Vector3 swingDirection = new Vector3(xAttack, 0, yAttack);

        cooldownMeter -= Time.deltaTime;

        //if (cooldownMeter <= 0f) canAttack = true; else canAttack = false; //Maybe tying the canAttack directly to the cooldown isn't the best idea

        if (swingDirection.magnitude > 0.1f)
        {
            targetAngle = Mathf.Atan2(swingDirection.x, swingDirection.z) * Mathf.Rad2Deg;
            if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) && canAttack && cooldownMeter <= 0f)
            {
                switch (stance)
                {
                    case 0:
                        stance = 1;
                        rotationTimer = 0.75f;
                        transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
                        Attack(0);
                        break;
                    case 1:
                        stance = 2;
                        rotationTimer = 0.75f;
                        transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
                        Attack(1);
                        break;
                    case 2:
                        stance = 1;
                        rotationTimer = 0.75f;
                        transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
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
    public void IncreaseBlood(float newBlood)
    {
        blood += newBlood;
    }
}

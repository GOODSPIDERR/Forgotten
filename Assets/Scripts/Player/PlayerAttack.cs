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
    public float blood = 0f;
    public float cooldown;
    float cooldownMeter;

    public Material bloodMaterial;
    PlayerInput playerInput;
    PlayerInputActions playerInputActions;
    public Slider slider;
    public VisualEffect bloodDrip;
    bool playBool = false;
    Vector3 initialPosition;
    AudioSource hitSound;
    public CinemachineVirtualCamera virtualCamera;
    public Execution execution;
    Animator animator;
    [HideInInspector] public Transform enemyThatCould;
    public GameObject deathVFX;
    public Text bloodText;

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

        initialPosition = slider.transform.localPosition;
        hitSound = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        #endregion
    }
    void Update()
    {
        float bloodDrain;
        bloodDrain = blood < 1f ? 0.25f : 0.5f;
        blood -= bloodDrain * Time.deltaTime;
        blood = Mathf.Clamp(blood, 0f, 1.5f);
        slider.value = blood;

        bloodText.text = (Mathf.Round(blood * 100f)).ToString() + "%";

        if (blood > 0) bloodDrip.enabled = true;
        else bloodDrip.enabled = false;

        bloodMaterial.SetFloat("Cleanliness_", 1f - blood);

        Vector2 attackVector = playerInputActions.Player.Attack.ReadValue<Vector2>();

        Debug.Log(attackVector);

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
    public void IncreaseBlood(float newBlood)
    {
        blood += newBlood;
        if (blood >= 1f)
        {
            slider.transform.localPosition = initialPosition;
            slider.transform.DOShakePosition(0.5f, 10f, 30, 90f);

        }
    }

    public void TakeDamage(GameObject damageSource)
    {
        float randomNumber = Random.Range(0.0f, 1.0f);
        if (randomNumber <= blood)
        {
            CinemachineBasicMultiChannelPerlin perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            perlin.m_AmplitudeGain = 5f;
            DOTween.To(() => perlin.m_AmplitudeGain, x => perlin.m_AmplitudeGain = x, 0f, 0.5f);
            hitSound.Play();
            Debug.Log("Damage Blocked!");
            blood -= 0.2f;
        }
        else
        {
            //Wow. Wtf lol
            //Actually this isn't too bad idk why I'm reacting like this
            execution.Execute();
            blood = 0f;
            canAttack = false;
            playerMovement.canMove = false;
            animator.SetTrigger("Death");
            Debug.Log("You're dead!");
            enemyThatCould = damageSource.transform;
            rotationTimer = 0f;
            animator.SetLayerWeight(1, 0f);


            Vector3 distance = new Vector3(transform.position.x - enemyThatCould.position.x, 0, transform.position.z - enemyThatCould.position.z).normalized;
            transform.rotation = Quaternion.LookRotation(-distance);

            var vfx = Instantiate(deathVFX, transform.position, Quaternion.LookRotation(-distance));
            enemyThatCould.GetComponent<MeshRenderer>().material = execution.white;
            transform.DOMove(transform.position + distance * 2f, 0.5f);
            vfx.transform.SetParent(transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bad"))
        {
            TakeDamage(other.gameObject);
        }
    }
}

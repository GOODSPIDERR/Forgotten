using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using Cinemachine;
using UnityEngine.VFX;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    PlayerMovement playerMovement;
    PlayerAttack playerAttack;
    public float blood = 0f;
    public Material bloodMaterial;
    public Slider slider;
    public VisualEffect bloodDrip;
    AudioSource hitSound;
    public CinemachineVirtualCamera virtualCamera;
    public Execution execution;
    Animator animator;
    [HideInInspector] public Transform enemyThatCould;
    public GameObject deathVFX;
    public Text bloodText;
    Vector3 initialPosition;
    
    //Post Processing Stuff
    public Volume volume;
    private Vignette vignette;

    private void Start()
    {
        #region Getters & Setters
        //Link
        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();

        //Don't need the input system here (yet)
        animator = GetComponent<Animator>();
        initialPosition = slider.transform.localPosition;
        hitSound = GetComponent<AudioSource>();
        #endregion

        //Post processing
        volume.profile.TryGet(out vignette);
    }

    private void Update()
    {
        //Before I forget, please learn how to use scriptable objects and start using them. 

        float bloodDrain;
        bloodDrain = blood < 1f ? 0.25f : 0.5f;
        blood -= bloodDrain * Time.deltaTime;
        blood = Mathf.Clamp(blood, 0f, 1.5f);
        slider.value = blood;

        bloodText.text = (Mathf.Round(blood * 100f)).ToString() + "%";

        if (blood > 0) bloodDrip.enabled = true;
        else bloodDrip.enabled = false;

        bloodMaterial.SetFloat("Cleanliness_", 1f - blood);

        vignette.intensity.value = blood * 0.25f;
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
        var randomNumber = Random.Range(0.0f, 1.0f);
        if (randomNumber <= blood)
        {
            var perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
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
            playerAttack.canAttack = false;
            playerMovement.canMove = false;
            animator.SetTrigger("Death");
            Debug.Log("You're dead!");
            enemyThatCould = damageSource.transform;
            playerAttack.rotationTimer = 0f;
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

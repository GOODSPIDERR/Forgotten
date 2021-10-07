using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{

    public float movementSpeed = 5f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    CharacterController controller;
    PlayerAttack playerAttack;
    public bool canMove;
    Animator animator;
    PlayerInput playerInput;
    PlayerInputActions playerInputActions;
    Vector3 direction;

    void Start()
    {
        canMove = true;
        controller = GetComponent<CharacterController>();
        playerAttack = GetComponent<PlayerAttack>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    void Update()
    {
        Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
        float xMovement = inputVector.x;
        float yMovement = inputVector.y;

        //Debug.Log(inputVector);

        direction = new Vector3(xMovement, 0, yMovement).normalized;

        animator.SetFloat("MoveSpeed", direction.magnitude);

        if (direction.magnitude >= 0.1f && canMove)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            if (playerAttack.stance == 0) transform.rotation = Quaternion.Euler(0f, angle, 0f);

            controller.Move((direction * movementSpeed * Time.deltaTime) + Vector3.up * -1);

            Vector3 localVelocity = transform.InverseTransformDirection(direction);

            //Debug.Log("Testing Solution: " + localVelocity);

            animator.SetFloat("VelocityX", localVelocity.x);
            animator.SetFloat("VelocityY", localVelocity.z);

        }
        else
        {
            animator.SetFloat("VelocityX", 0f);
            animator.SetFloat("VelocityY", 0f);
        }

    }
    public void Dodge()
    {
        if (direction.magnitude >= 0.1f && canMove)
        {
            animator.SetTrigger("Roll");
            canMove = false;
            playerAttack.rotationTimer = 0f;
            transform.rotation = Quaternion.Euler(0, Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg, 0);
            transform.DOMove(transform.position + new Vector3(direction.x, 0, direction.z) * 3, 0.5f, false).SetEase(Ease.OutSine).OnComplete(() => { canMove = true; });
        }
    }
}

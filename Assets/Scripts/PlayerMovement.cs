using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    float movementSpeed = 5f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    CharacterController controller;
    PlayerAttack playerAttack;
    public bool canMove;
    Animator animator;

    void Start()
    {
        canMove = true;
        controller = GetComponent<CharacterController>();
        playerAttack = GetComponent<PlayerAttack>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float xMovement = Input.GetAxisRaw("Horizontal");
        float yMovement = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(xMovement, 0, yMovement).normalized;

        animator.SetFloat("MoveSpeed", direction.magnitude);

        if (direction.magnitude >= 0.1f && canMove)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            if (playerAttack.stance == 0) transform.rotation = Quaternion.Euler(0f, angle, 0f);

            controller.Move((direction * movementSpeed * Time.deltaTime) + Vector3.up * -1);

            Vector2 movementVector = new Vector2(direction.x, direction.z);
            Vector2 rotationVector = DegreeToVector2(transform.rotation.eulerAngles.y);

            Vector2 differenceVector = new Vector2(movementVector.x - rotationVector.x, movementVector.y - rotationVector.y);

            Vector2 absoluteVector = new Vector2(differenceVector.x, direction.magnitude);

            //Vector2 targetVector = new Vector2()

            Debug.Log("Movement Vector: " + movementVector);
            Debug.Log("Rotation Vector: " + rotationVector);

            Debug.Log("Testing Solution: " + (new Vector2(0, direction.magnitude)));

            animator.SetFloat("VelocityX", 0);
            animator.SetFloat("VelocityY", direction.magnitude);

            Vector2 RadianToVector2(float radian)
            {
                return new Vector2(Mathf.Sin(radian), Mathf.Cos(radian));
            }

            Vector2 DegreeToVector2(float degree)
            {
                return RadianToVector2(degree * Mathf.Deg2Rad);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetTrigger("Roll");
                canMove = false;
                playerAttack.rotationTimer = 0f;
                transform.rotation = Quaternion.Euler(0, Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg, 0);
                transform.DOMove(transform.position + new Vector3(direction.x, 0, direction.z) * 3, 0.5f, false).SetEase(Ease.OutSine).OnComplete(() => { canMove = true; });
            }

        }
    }
}

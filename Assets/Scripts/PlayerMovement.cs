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

    void Start()
    {
        canMove = true;
        controller = GetComponent<CharacterController>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    void Update()
    {
        float xMovement = Input.GetAxisRaw("Horizontal");
        float yMovement = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(xMovement, 0, yMovement).normalized;

        if (direction.magnitude >= 0.1f && canMove)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            if (playerAttack.stance == 0) transform.rotation = Quaternion.Euler(0f, angle, 0f);

            controller.Move((direction * movementSpeed * Time.deltaTime) + Vector3.up * -1);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                canMove = false;
                playerAttack.rotationTimer = 0f;
                transform.rotation = Quaternion.Euler(0, Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg, 0);
                transform.DOMove(transform.position + new Vector3(direction.x, 0, direction.z) * 3, 0.5f, false).SetEase(Ease.OutSine).OnComplete(() => { canMove = true; });
            }
        }
    }
}

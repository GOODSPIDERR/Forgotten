using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int stance = 0;
    public GameObject fastSwing1, fastSwing2;
    public float rotationTimer = 0f;
    float targetAngle = 0f;
    public bool canAttack;

    void Start()
    {
        canAttack = true;
    }

    void Update()
    {
        float xAttack = Input.GetAxisRaw("HAttack");
        float yAttack = Input.GetAxisRaw("VAttack");

        Vector3 swingDirection = new Vector3(xAttack, 0, yAttack);

        if (swingDirection.magnitude > 0.1f)
        {
            targetAngle = Mathf.Atan2(swingDirection.x, swingDirection.z) * Mathf.Rad2Deg;
            if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) && canAttack)
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
}

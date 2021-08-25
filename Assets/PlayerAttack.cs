using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int stance = 0;
    public GameObject fastSwing;
    float rotationTimer = 0f;
    float targetAngle = 0f;
    void Start()
    {

    }

    void Update()
    {
        float xAttack = Input.GetAxisRaw("HAttack");
        float yAttack = Input.GetAxisRaw("VAttack");

        Vector3 swingDirection = new Vector3(xAttack, 0, yAttack);

        if (swingDirection.magnitude > 0.1f)
        {
            targetAngle = Mathf.Atan2(swingDirection.x, swingDirection.z) * Mathf.Rad2Deg;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            stance = 1;
            rotationTimer = 0.75f;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
            var swing = Instantiate(fastSwing, transform.position + transform.forward + transform.up, transform.rotation);
            swing.transform.parent = gameObject.transform;

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



    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    PlayerMovement playerMovement;
    PlayerAttack playerAttack;

    void Start()
    {
        #region Getters & Setters
        //Link
        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();

        //Don't need the input system here (yet)
        #endregion
    }

    void Update()
    {
        //Before I forget, please learn how to use scriptable objects and start using them. 
        //And split PlayerAttack.cs pls and thank you
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingScript : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 0.2f);
    }

    /*
    void Update()
    {
        Was used to destroy old instances; with the cooldown implemented it's not needed anymore
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(gameObject);
        }   
    }
    */
}

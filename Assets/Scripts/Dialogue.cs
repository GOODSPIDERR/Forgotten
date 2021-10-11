using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public int index = 0;
    // Start is called before the first frame update
    void OnEnable()
    {
        index++;
    }

    void Update()
    {

    }

    public void Increment()
    {
        index++;
    }
}

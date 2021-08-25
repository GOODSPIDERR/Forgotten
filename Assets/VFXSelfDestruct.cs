using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXSelfDestruct : MonoBehaviour
{

    void Start()
    {
        Destroy(gameObject, 4);
    }

}

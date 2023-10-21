using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shreder : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Power");
        Destroy(other.gameObject);

    }

}

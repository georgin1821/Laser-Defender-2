using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] float speedOfSpeen = 1f;
    void Update()
    {
        transform.Rotate(0, speedOfSpeen * Time.deltaTime, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollistionDetection : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("NewBlock"))
        {
            Debug.Log("Collision Detected");
        }
    }
}

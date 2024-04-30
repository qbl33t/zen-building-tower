using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorRemover : MonoBehaviour
{
    void Update()
    {
        if (gameObject != null && transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }
}

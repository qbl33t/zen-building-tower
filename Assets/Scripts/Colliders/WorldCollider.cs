using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCollider : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (gameObject != null && transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }
}

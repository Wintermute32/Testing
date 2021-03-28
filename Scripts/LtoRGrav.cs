using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LtoRGrav : MonoBehaviour
{
    Vector3 gravity;
    void Start()
    {
        gravity = Physics2D.gravity;
    }

    void FixedUpdate()
    {
        Physics2D.gravity = gravity;
        gravity.x = .7f;
        gravity.y = 0;
        gravity.z = 0;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseCollider : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        
        Level level = FindObjectOfType<Level>();
        if (collision.gameObject.tag == "Ball")
        {
            StartCoroutine(level.BallReset(collision));
        }
       
        Destroy(collision.gameObject);

    }

}

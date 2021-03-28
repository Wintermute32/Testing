using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMidCollider : MonoBehaviour
{
    VideoPlayer videoPlayer;
    public void OnTriggerEnter2D(Collider2D collider)
    {
        ChangeBlockAlpha(collider, collider.gameObject.tag);
    }
    private void ChangeBlockAlpha(Collider2D collision, string collisionTag)
    {
        if (collisionTag != "Base Brick" && collisionTag != "Ball")
        {

            var sA = collision.gameObject.GetComponent<SpriteRenderer>().color;
            var sC = collision.gameObject.GetComponent<SpriteRenderer>();
            sC.color = new Color(255, 255, 255);

            sA.a = .4f;

            collision.gameObject.GetComponent<SpriteRenderer>().color = sA;

            var dieText = FindObjectOfType<DieTextBehavior>();
            dieText.enabled = true;
            StartCoroutine(dieText.RandomizeSymbol());
            dieText.enabled = false;
        }
    }

    public IEnumerator EnableVideo()
    {
        videoPlayer = FindObjectOfType<VideoPlayer>();

        if (videoPlayer.enabled == false)
        {
            videoPlayer.enabled = true;
            yield return new WaitForSeconds(.4f);
        }

        videoPlayer.enabled = false;
    }
}

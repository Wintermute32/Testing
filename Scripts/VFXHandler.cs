using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXHandler : MonoBehaviour
{
    public VFX vfx;
    ParticleSystem blastParticle;
    Ball ball;
    BallInputManager ballInputManager;

    void Start()
    {
        FindBallGameObject();
    }

    // Update is called once per frame
    void Update()
    {
        FindBallGameObject();
    }

    private void Testing_OnButtonPressed(object sender, EventArgs e)
    {
        Debug.Log("Event Halder Effect Triggerd!");
        Instantiate(blastParticle, ball.transform);
        blastParticle.Play();
    }

    public void FindBallGameObject()
    {
        if (!ballInputManager)
        {
            ballInputManager = FindObjectOfType<BallInputManager>();
            ballInputManager.OnButtonPressed += Testing_OnButtonPressed;
            ball = FindObjectOfType<Ball>();
            blastParticle = vfx.blastparticle;
        }
    }

}

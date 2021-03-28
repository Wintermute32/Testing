using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Ball : MonoBehaviour
{
    //config parameters 
    //[SerializeField] Paddle paddle1;
    [SerializeField] float xPush = 2f;
    [SerializeField] float yPush = 15f;
    //[SerializeField] AudioClip[] ballSounds;
    [SerializeField] public float randomFactor = 0.2f;
    //state
    Vector2 paddleToBallVector;

    public bool hasStarted { get; set; }

    //Cached component references
    AudioSource myAudioSource;
    Rigidbody2D myRigidBody2D;
    Paddle paddle1;

    // Start is called before the first frame update
    void Start()
    {
        paddle1 = FindObjectOfType<Paddle>();
        paddleToBallVector = transform.position - paddle1.transform.position;
        myAudioSource = GetComponent<AudioSource>();
        myRigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasStarted)
        {
            LockBallToPaddle();
            LaunchOnMouseClick();
        }
    }
    private void LaunchOnMouseClick()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.Space))
        {
            hasStarted = true;
            myRigidBody2D.velocity = new Vector2(xPush, yPush);
        }
    }
    public void LockBallToPaddle()
    {
        Vector2 paddlePos = new Vector2(paddle1.transform.position.x, paddle1.transform.position.y);
        transform.position = paddlePos + paddleToBallVector;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Vector2 inNormal = collision.contacts[0].normal;
        //Vector2 newVelocity = Vector2.Reflect(myRigidBody2D.velocity, inNormal);

        myRigidBody2D.velocity += new Vector2(0, UnityEngine.Random.Range(0, randomFactor));


        if (hasStarted)
        {
            //AudioClip clip = ballSounds[UnityEngine.Random.Range(0, ballSounds.Length)];
            //myAudioSource.PlayOneShot(clip);
            //myRigidBody2D.velocity += velocityTweak;
        }

    }

}

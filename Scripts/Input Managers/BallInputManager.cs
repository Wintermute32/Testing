using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInputManager : MonoBehaviour
{
    [SerializeField] float yPush;
    [SerializeField] float xPush;
    [SerializeField] float pressBuffer;
    Rigidbody2D rigid2D;

    Vector2 velocity;
    bool collided;
    string input;
    public VFX vfx;
    float collisionTime;
    float timeDifference;
    float minSpeed;

    Dictionary<string, Vector2> inputDict;

    public event EventHandler OnButtonPressed;

    void Start()
    {
        NewBallSetup();
        minSpeed = gameObject.GetComponent<Accelerator>().minSpeed;
    }

    private void NewBallSetup()
    {
        rigid2D = gameObject.GetComponent<Rigidbody2D>();
        Vector2 forceVectorY = new Vector2(0, yPush);
        Vector2 forceVectorX = new Vector2(xPush, 0);
        Vector2 forceVectorNegY = new Vector2(0, -yPush);
        Vector2 forceVectorNegX = new Vector2(-xPush, 0);
        Vector2 forceVectorAngle = new Vector2(xPush, yPush);
        Vector2 forceVectorAngleNeg = new Vector2(-xPush, yPush);

        inputDict = new Dictionary<string, Vector2>(){
            {"w", forceVectorY},{"s", forceVectorNegY},{"a", forceVectorNegX },
            {"d", forceVectorX}, {"wd", forceVectorAngle}, {"wa", forceVectorAngleNeg} };
    }

    private void Update()
    {
        CheckForButtonInput();
        SlowBall();
    }
    void FixedUpdate()
    {
        //Event Check would go here
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        collided = true;
        collisionTime = Time.time;
    }
    public void CheckForButtonInput()
    {
        if (Input.anyKeyDown)
        {
            input = Input.inputString;;
            StartCoroutine(ResetPushTime());
        }

        if (inputDict.ContainsKey(input))
            VelocityManipulate(input, Time.time);
    }
    public void VelocityManipulate(string pressedKeys, float pressedTime)
    {
        timeDifference = Mathf.Abs(collisionTime - pressedTime);

        if (timeDifference <= pressBuffer && collided)
        {
            rigid2D.velocity += (inputDict[pressedKeys]);
            OnButtonPressed?.Invoke(this, EventArgs.Empty);
            input = null;
        }

        collided = false;

    }

    public IEnumerator ResetPushTime() //if ball hasn't collided yet, reset input
    {
        yield return new WaitForSeconds(pressBuffer);
         input = null;
    }

    public void SlowBall()
    {
        //bool notTooSlow = rigid2D.velocity.x > minSpeed && rigid2D.velocity.y > minSpeed;

        if (Input.GetMouseButton(1))
        {
            Debug.Log("Should be slowing down");
            velocity = rigid2D.velocity * .99f;
            rigid2D.velocity = velocity;
            //OnButtonPressed?.Invoke(this, EventArgs.Empty);
        }
    }
}

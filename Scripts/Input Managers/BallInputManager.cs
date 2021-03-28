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

    ParticleSystem particle;
    bool collisionBuffer;
    public VFX vfx;
    float collisionTime;
    float timeDifference;

    Dictionary<char, Vector2> inputDict;

    public event EventHandler OnButtonPressed;

    void Start()
    {
        NewBallSetup();
    }

    private void NewBallSetup()
    {
        rigid2D = gameObject.GetComponent<Rigidbody2D>();
        Vector2 forceVectorY = new Vector2(0, yPush);
        Vector2 forceVectorX = new Vector2(xPush, 0);
        Vector2 forceVectorNegY = new Vector2(0, -yPush);
        Vector2 forceVectorNegX = new Vector2(-xPush, 0);

        inputDict = new Dictionary<char, Vector2>(){
            {'w', forceVectorY},{'s', forceVectorNegY},{'a', forceVectorNegX },
            {'d', forceVectorX}};
    }

    private void Update()
    {
        CheckForButtonInput();
    }
    void FixedUpdate()
    {
        //Event Check would go here
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        collisionTime = Time.time;
    }
    public void CheckForButtonInput() //THIS MUST be in update. Trying Input.inputstring to get pressed key then check if its in dict
    {
        if (Input.GetKeyDown(KeyCode.W))
            VelocityManipulate('w', Time.time);    

        if (Input.GetKeyDown(KeyCode.S))
            VelocityManipulate('s', Time.time);
            

        if (Input.GetKeyDown(KeyCode.A))
            VelocityManipulate('a', Time.time);

        if (Input.GetKeyDown(KeyCode.D))
            VelocityManipulate('d', Time.time);
    }
    public void VelocityManipulate(char pressedKeys, float pressedTime)
    {
        //if (inputDict.ContainsKey(pressedKeys))

        timeDifference = Mathf.Abs(collisionTime - pressedTime);
        if (timeDifference < pressBuffer)
        {
            //Debug.Log("Ping Fired!" + inputDict[pressedKeys]);

            rigid2D.velocity += (inputDict[pressedKeys]);
            OnButtonPressed?.Invoke(this, EventArgs.Empty);
        }

    }
    public IEnumerator ButtonPressPeriod()
    {
        collisionBuffer = true;
        yield return new WaitForSeconds(pressBuffer);
        collisionBuffer = false;
    }
}

using UnityEngine;

public class Accelerator : MonoBehaviour
{
    [Tooltip("min and max speed are still affected by drag, unless drag = 0")]
    [SerializeField] float minSpeed;
    [SerializeField] float maxSpeed;
    public float ballSpeedX { get; private set; }
    public float ballSpeedY { get; private set; }
    Ball ball;
    Rigidbody2D ballRigid;
    LaunchController launchController;

    void Start()
    {
        ball = gameObject.GetComponent<Ball>();
        ballRigid = gameObject.GetComponent<Rigidbody2D>();
        launchController = FindObjectOfType<LaunchController>();
    }
    void FixedUpdate()
    {
        if (ball.hasStarted && !launchController.isCharging)
        {
           CheckBallSpeed();
        }
    }
    public void CheckBallSpeed()
    {
        float velXAbs = Mathf.Abs(ballRigid.velocity.x);
        float velYAbs = Mathf.Abs(ballRigid.velocity.y);

        if (ballRigid.velocity.x >= maxSpeed || ballRigid.velocity.y >= maxSpeed) //an easy way to cap max velocity
            ballRigid.velocity = Vector3.ClampMagnitude(ballRigid.velocity, maxSpeed);

       if (velXAbs > velYAbs && velXAbs < minSpeed) //our stupid way to cap min velocity.
       {
            if (ballRigid.velocity.x > 0)
                ballRigid.AddForce(new Vector2(minSpeed - ballRigid.velocity.x, 0));

            if (ballRigid.velocity.x < 0)
                ballRigid.AddForce(new Vector2(-(minSpeed + ballRigid.velocity.x), 0));
       }

        if (velXAbs < velYAbs && velYAbs < minSpeed)
        {
            if (ballRigid.velocity.y > 0)
                ballRigid.AddForce(new Vector2(0, minSpeed - ballRigid.velocity.y));

            if (ballRigid.velocity.y < 0)
                ballRigid.AddForce(new Vector2(0, -(minSpeed + ballRigid.velocity.y)));
        }

        //Debug.Log(ballRigid.velocity);

    }
           
 }

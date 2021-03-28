using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Level : MonoBehaviour
{
    [SerializeField] Paddle paddle;
    [SerializeField] Ball ball;
    [SerializeField] float ballResetTime;
 
    //cached reference
    SceneLoader sceneLoader;

   private void Start()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
        
    }    

   public void CountBlocks()
    {
        //breakableBlocks++; 
    }
    public IEnumerator BallReset(Collider2D collision)
    {
        var paddleCollider = paddle.GetComponent<PolygonCollider2D>();
        var ballYOffset = ball.GetComponent<CircleCollider2D>().bounds.extents.y;
        Destroy(collision.gameObject);

        yield return new WaitForSeconds(ballResetTime);

        Vector3 ballReturnOffset = new Vector3(paddleCollider.bounds.center.x, paddleCollider.bounds.extents.y * 2 + ballYOffset, 0);

        var newBall = Instantiate(ball, ballReturnOffset, transform.rotation);
        newBall.hasStarted = false;

    }
}

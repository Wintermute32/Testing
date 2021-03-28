using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Paddle : MonoBehaviour
{
    [SerializeField] float screenWidthInUnits = 16f;
    [SerializeField] float maxX;
    [SerializeField] float minX;
    [SerializeField] float paddleMoveSpeed;

    //cached references
    private Ball blueBalls;
    private GameSession gameSession;

    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();

    }
    void Update()
    {
        MovePaddle();
    }

    private void MovePaddle()
    {
        Vector2 paddlePos = new Vector2(transform.position.x, transform.position.y);
        
        paddlePos.x = Mathf.Clamp(GetXPos(), minX, maxX);
        
        transform.position = paddlePos;

    }

    private float GetXPos()
    {
        if (gameSession.IsAutoPlayEnabled())
        {
            return blueBalls.transform.position.x;
        }
        else
        {
            //return Input.mousePosition.x / Screen.width * screenWidthInUnits;
            //float axis = Input.GetAxis("Horizontal");

             return transform.position.x + (Input.GetAxis("Horizontal")
                    * screenWidthInUnits * Time.deltaTime) * paddleMoveSpeed;
        }
 
    }
}

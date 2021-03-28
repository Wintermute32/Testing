using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaunchController : MonoBehaviour
{
    [SerializeField] Slider chargeSlider;
    private Ball blueBalls;
    float deltaValue = 0;
    public bool isCharging { get; private set; }
    private bool isResting;
    float randomFactActual;
    void FixedUpdate()
    {
        CheckForButtonHold();
        ChargeSlider();
    }
    public void CheckForButtonHold()
    {
        isCharging = Input.GetMouseButton(0);

        if (!isCharging)
        {
            ReleaseBall(blueBalls);
        }

    }
    public void OnCollisionEnter2D(Collision2D collider)
    {
        blueBalls = collider.gameObject.GetComponent<Ball>();
        if (collider.gameObject.GetComponent<Ball>() && isCharging && chargeSlider.value <= chargeSlider.minValue)
        {
            isResting = true;
            collider.gameObject.transform.position = gameObject.transform.position;
            CatchBall(blueBalls);
        }
    }
    public void CatchBall(Ball ball)
    {
        randomFactActual = ball.randomFactor;
        ball.randomFactor = 0;
        ball.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        ball.transform.parent = gameObject.transform;
    }
    public void ReleaseBall(Ball ball)
    {
        if (isResting)
        {
            Debug.Log("Firing");
            ball.randomFactor = randomFactActual;
            ball.GetComponent<Rigidbody2D>().velocity = new Vector2(1f, 10f * ChargeSlider());
            ball.transform.parent = null;
            isResting = false;
        }
    }
    public float ChargeSlider()
    {
        if (isResting)
        {
            Debug.Log("I'm Called Charging!");
            deltaValue += Time.deltaTime;
            chargeSlider.value += deltaValue + Mathf.Sqrt(chargeSlider.value / 100);
        }
        else
            chargeSlider.value -= Time.deltaTime * 2;

        return chargeSlider.value;
    }


}

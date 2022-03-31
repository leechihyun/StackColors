using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float roadWidth = 10f;
    public float forwardSpeed = 3f;

    public float forwardAcceleration = 1f;

    public Vector3 velocity;


    public Vector3 Move(){
        Vector3 deltaPosition = Vector3.zero;
        Vector3 desiredPosition = transform.position;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            velocity.x = touch.deltaPosition.x / (float)Screen.width * 1.2f * roadWidth;
        }
        velocity.y = 0f;
        velocity.z = Mathf.Lerp(velocity.z, forwardSpeed, Time.deltaTime * forwardAcceleration);

        desiredPosition.x = transform.position.x + velocity.x;
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, -roadWidth *0.5f+0.5f, roadWidth * 0.5f - 0.5f);
        desiredPosition.z = transform.position.z + velocity.z * Time.deltaTime;

        deltaPosition = desiredPosition - transform.position;

        transform.position = desiredPosition;

        return deltaPosition;
    }



}

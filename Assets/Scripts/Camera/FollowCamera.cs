using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
  
    [SerializeField] private float speedStart = 6f;
    private float speed;
    private float speedX, speedZ;

    void Start()
    {
        speed = speedStart;
    }

    void FixedUpdate()
    {
        speedX = 0;
        speedZ = 0;

        if (Input.GetKey(KeyCode.D))
        {
            if (speedX == 0)
                speedX += speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (speedX == 0)
                speedX -= speed;
        }
        if (Input.GetKey(KeyCode.W))
        {
            if (speedZ == 0)
                speedZ += speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (speedZ == 0)
                speedZ -= speed;
        }
        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A))
        {
            speedX = 0;
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            speedZ = 0;
        }
        
    }
}
   



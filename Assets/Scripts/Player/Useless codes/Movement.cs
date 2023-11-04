using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private float speedStart = 6f;
    [SerializeField] private float speed;
    private float speedX, speedZ;
    [SerializeField] private float ctrlMod = 0.5f;
    [SerializeField] private float shiftMod = 1.5f;
    [SerializeField] private float diagonMod = 1.5f;

    private void Awake()
    {
        speed = speedStart;
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) && ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))))
        {
            speed = speedStart * ctrlMod * shiftMod;
        }
        else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            speed = speedStart * ctrlMod;
        }
        else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            speed = speedStart * shiftMod;
        }
        else
        {
            speed = speedStart;
        }
        //if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W))
        //{
        //    rb.velocity = new Vector3(speed / diagonMod, 0, speed / diagonMod);
        //}
        //else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S))
        //{
        //    rb.velocity = new Vector3(speed / diagonMod, 0, -speed / diagonMod);
        //}
        //else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W))
        //{
        //    rb.velocity = new Vector3(-speed / diagonMod, 0, speed / diagonMod);
        //}
        //else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S))
        //{
        //    rb.velocity = new Vector3(-speed / diagonMod, 0, -speed / diagonMod);
        //}
        //else if (Input.GetKey(KeyCode.D))
        //{
        //    rb.velocity = new Vector3(speed, 0, 0);
        //}
        //else if (Input.GetKey(KeyCode.A))
        //{
        //    rb.velocity = new Vector3(-speed, 0, 0);
        //}
        //else if (Input.GetKey(KeyCode.W))
        //{
        //    rb.velocity = new Vector3(0, 0, speed);
        //}
        //else if (Input.GetKey(KeyCode.S))
        //{
        //    rb.velocity = new Vector3(0, 0, -speed);
        //}
        //else
        //{
        //    rb.velocity = new Vector3(0, 0, 0);
        //}

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
        if (speedX != 0 && speedZ != 0)
        {
            rb.velocity = new Vector3(speedX / diagonMod, 0, speedZ / diagonMod);
        }
        else
        {
            rb.velocity = new Vector3(speedX, 0, speedZ);
        }
    }

}
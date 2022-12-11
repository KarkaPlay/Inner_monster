using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mousefollow : MonoBehaviour
{
    private CharacterController controller;
    public Transform cam;
    public float speed = 10f;
    public float gravity = -9;
    public float turnSmothTime = 0.6f;
    private float jspeed = 0;
    float turnSmoothVelocity;
    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        float hor = 0;
        float ver = 0;
        if (controller.isGrounded)
        {
             hor = Input.GetAxis("Horizontal");
            ver = Input.GetAxis("Vertical");
        }
        jspeed += gravity * Time.deltaTime * 3f;
        Vector3 dir = new Vector3(0, jspeed * Time.deltaTime, 0);
        controller.Move(dir);
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed += 4;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed -= 4;
        }
        Vector3 direction = new Vector3(hor, 0, ver).normalized;
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }
}

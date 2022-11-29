using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    public Animator go;
    private int _state;
    private int _jump;
    void Start()
    {
        go = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.LeftShift))
                _state = 2;
            else
                _state = 1;
        }
        else _state = 0;
        if (Input.GetKey(KeyCode.Space))
        {
            if ((_state == 2) || (_state == 1))
                _jump = 2;
            else _jump = 1;
        }
        else _jump = 0;
        go.SetInteger("jump", _jump);
        go.SetInteger("state", _state);
    }
}

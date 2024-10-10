using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]

public class Movement : MonoBehaviour
{
    [Range(0, 10)]
    public float speed = 5;

    [Range(0, 10)]
    public float sprintSpeed = 10;
    [Range(0, 10)]
    public float walkSpeed = 5;

    [Range(0, 5)]
    public float sensitivity = 0.2f;

    public Transform head;



    Canvas canvas;
    private Vector3 lastMousePos;
    private float horizontalAngle;
    private float verticalAngle;
    private Rigidbody rb;
    private Camera cam;
    public Vector3 velocity;
    public float gravity = 20;
    public bool isGrounded;
    public bool wannaJump;
    public float tryJumpTime;
    public double forgiveness;
    public Vector3 normal;
    private bool gameHasEnded = false;


    private void Start()
    {
        cam = Camera.main;
       Cursor.visible = false;
       Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        
    }

    private void Update()
    {
        Sprinting();
        Rotation();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            wannaJump = true;
            tryJumpTime = Time.time;
        }

        if (Time.time > tryJumpTime + forgiveness) wannaJump = false;

        if (rb.position.y < -1f) GameOver();
    }

    public void GameOver()
    {
        if(gameHasEnded == false)
        {
            gameHasEnded = true;
            Restart();
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void FixedUpdate()
    {
        var ray = new Ray(transform.position, Vector3.down);
        isGrounded = Physics.Raycast(ray, out var hit, 1.3f);
        normal = hit.normal;
        if (normal == Vector3.zero)  normal = Vector3.up;



        Moving();
        Gravity();
        Jumping();
        
        
        rb.velocity = velocity;
    }

 

    private void Sprinting()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = sprintSpeed;
            cam.fieldOfView = 80;
        }
        else
        {
            speed = walkSpeed;
            cam.fieldOfView = 60;

        }
    }

    private void Gravity()
    {
        if (isGrounded)
        {
            velocity.y = 0;
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
        }
    }


    private void Jumping()
    {
        if (wannaJump && isGrounded)
        {
            velocity.y = 10;
            wannaJump = false;
        }
    }

    private void Moving()
    {
        var input = (transform.right * Input.GetAxisRaw("Horizontal") * Time.deltaTime +
             transform.forward * Input.GetAxisRaw("Vertical") * Time.deltaTime);

        input.Normalize();

        input = Vector3.ProjectOnPlane(input, normal);
        input.Normalize();
        input *= speed;

        velocity.x = input.x; 
        velocity.z = input.z;

        if (isGrounded) velocity.y = input.y;
    }

    private void Rotation()
    {
        verticalAngle += Input.GetAxisRaw("Mouse Y") * -sensitivity;
        horizontalAngle += Input.GetAxisRaw("Mouse X") * sensitivity;

        //do not allow salto
        verticalAngle = Mathf.Clamp(verticalAngle, -90, 90);

       //transform.rotation
        transform.eulerAngles = new Vector3(0, horizontalAngle, 0);

        //head rotation
        head.localEulerAngles = new Vector3(verticalAngle, 0, 0);
    }
}

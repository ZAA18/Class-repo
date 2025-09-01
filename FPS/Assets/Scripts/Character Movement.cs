using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.ProBuilder.MeshOperations;
public class CharacterMovement : MonoBehaviour
{
    private CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpheight = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundmask;

    Vector3 velocity;
    bool isGrounded;
    bool isMoving;
    private Vector3 lastposition = new Vector3(0f,0f,0f);


    [Header("facebook")]
        public string Name = "";
    public string Surname = "";

    
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundmask);
        if (isGrounded && velocity.y < 0)
        { velocity.y = -2f; }

        float X = Input.GetAxis("Horizontal");
        float Z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * X + transform.forward * Z;

        // moving the player
        controller.Move(move * speed * Time.deltaTime);

        // checking jumping condition
        if (Input.GetButtonDown("Jump") && isGrounded)
            // jump
        { velocity.y = Mathf.Sqrt(jumpheight * -2f * gravity);
        }

        //falling down 
        velocity.y += gravity * Time.deltaTime;

        // exectuting the jump 
        controller.Move(velocity * Time.deltaTime);

        if (lastposition != gameObject.transform.position && isGrounded == true)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

            lastposition = gameObject.transform.position;
    }
}

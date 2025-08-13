using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class fpcontroller : MonoBehaviour
{

    [Header("movement Settings")]
    public float moveSpeed = 5f;
    public float Gravity = -9.81f;

    [Header("Look Settings")]
    public Transform cameraTransform;
    public float looksensitivity = 2f;
    public float verticalLookLimit = 90f;

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private float VerticalRotation = 0f;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // update function
    void Update()
    {

        Handlemovement();
        HandleLook();
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void Handlemovement()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)

            velocity.y = -2f;
        velocity.y += Gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }

    public void HandleLook()
    {
        float mouseX = lookInput.x + looksensitivity;
        float mouseY = lookInput.y + looksensitivity;
        VerticalRotation -= mouseY;
        VerticalRotation = Mathf.Clamp(VerticalRotation, -verticalLookLimit, verticalLookLimit);
        cameraTransform.localRotation = Quaternion.Euler(VerticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

}

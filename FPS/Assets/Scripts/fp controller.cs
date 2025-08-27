using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class FPCONTROLLER : MonoBehaviour
{

    [Header("movement Settings")]
    public float moveSpeed = 5f;
    public float Gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Look Settings")]
    public Transform cameraTransform;
    public float looksensitivity = 2f;
    public float verticalLookLimit = 90f;

    [Header("Shooting")]
    public GameObject bulletprefab;
    public Transform gunpoint;
    public float bulletvelocity = 1000;

    [Header("Crouch")]
    public float crouchheight = 1f;
    public float standheight = 2f;
    public float crouchspeed = 2.5f;
    private float originalmovespeed;

    [Header("Health System")]
    [SerializeField] float maxhealth = 100;
    float currentHealth;

    [Header("Pickup Settings")]
    public float pickupRange = 3f;
    public Transform holdpoint;
    private PickUpObject heldObject;


    [SerializeField] private HEALTHSYTEM healthbar;


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
       // currentHealth = maxhealth;
       // healthbar.UpdatehealthBar( maxhealth, currentHealth);
    }

    // update function
   private void Update()
    {

        Handlemovement();
        HandleLook();

        if (heldObject !=null)
        {
            heldObject.MoveToHoldPoint(holdpoint.position);
        }
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && controller.isGrounded)
        { velocity.y = Mathf.Sqrt(jumpHeight * -2f * Gravity); }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        { Shoot(); }
    }

    public void OnCrouch (InputAction.CallbackContext context)
    {
        if (context.performed)
        { controller.height = crouchheight;
            moveSpeed = crouchspeed;
        }
        else if (context.canceled)
        {
            controller.height = standheight;
            moveSpeed = originalmovespeed;
        }
    }

    public void OnPickUp(InputAction. CallbackContext context)
    {
        if (!context.performed) return;

        if (heldObject == null)
        {

            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
            {
                PickUpObject pickUp = hit.collider.GetComponent<PickUpObject>();
                if (pickUp !=null)
                {
                    pickUp.PickUp(holdpoint);
                    heldObject = pickUp;

                }

            }
        }
        else
        { heldObject.Drop();
          heldObject = null;

            

        }
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
        float mouseX = lookInput.x * looksensitivity;
        float mouseY = lookInput.y * looksensitivity;
        VerticalRotation -= mouseY;
        VerticalRotation = Mathf.Clamp(VerticalRotation, -verticalLookLimit, verticalLookLimit);
        cameraTransform.localRotation = Quaternion.Euler(VerticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void Shoot()
    {
        if (bulletprefab != null && gunpoint != null)
        { GameObject bullet = Instantiate(bulletprefab, gunpoint.position, gunpoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            if (rb !=null)
            { rb.AddForce(gunpoint.forward * bulletvelocity, ForceMode.Impulse); // adjust force value as needed
            }
        }
    }
}


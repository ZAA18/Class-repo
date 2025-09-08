using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using JetBrains.Annotations;
using UnityEngine.Rendering.Universal.Internal;

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
    public float bulletvelocity = 500;
    // for the particle system
    public GameObject fire;
    public GameObject HitPoint1;
    public GameObject HitPoint2;
    public GameObject HitPoint3;
//new code


    [Header("Crouch")]
    public float crouchheight = 1f;
    public float standheight = 2f;
    public float crouchspeed = 2.5f;
    private float originalmovespeed;

    [Header("Health System")]
    [SerializeField] float maxhealth = 2000;
    float currentHealth;

    [Header("Pickup Settings")]
    public float pickupRange = 3f;
    public Transform holdPoint;
    private PickUpObject heldObject;

    [Header("Throw Settings")]
    public float throwForce = 10f;
    public float throwUpwardBoost = 1f;

    [Header("Door System")]
    public float interactRange = 3f;


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
            heldObject.MoveToHoldPoint(holdPoint.position);
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
        { Fire(); }
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
                    pickUp.PickUp(holdPoint);
                    heldObject = pickUp;

                }

            }
        }
        else
        { heldObject.Drop();
          heldObject = null;

            

        }
    }

    public void OnThrow(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (heldObject == null) return;

        Vector3 dir = cameraTransform.forward;
        Vector3 impulse = dir * throwForce + Vector3.up * throwUpwardBoost;

       // heldObject.Throw(impulse); // pickup object (Script) does not contain a throw function
        heldObject = null;

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

   /* private void Shoot()
    {
        if (bulletprefab != null && gunpoint != null)
        { GameObject bullet = Instantiate(bulletprefab, gunpoint.position, gunpoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            if (rb !=null)
            { rb.AddForce(gunpoint.forward * bulletvelocity, ForceMode.Impulse); // adjust force value as needed
            }
        }
    }
   */

    public void OnInteract(InputAction.CallbackContext context)
    {

        if (!context.performed) return;

        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {

            //Only allow objects
            if (hit.collider.CompareTag("Switchable"))
            {

                var switcher = hit.collider.GetComponent<DoorSystem>();
                if (switcher != null)
                {
                    switcher.ToggleMaterial();
                }
            }
        }
             
     
    
    }

    private  void Fire()
    {
        RaycastHit hit;
        if(Physics.Raycast(gunpoint.position , transform.TransformDirection(Vector3.forward) , out hit , 100))

        { Debug.DrawRay(gunpoint.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);


            GameObject a = Instantiate(fire, gunpoint.position, Quaternion.identity);

            Destroy(a, 1);

            if (hit.transform.gameObject.CompareTag("Target"))
            { GameObject b = Instantiate(HitPoint1, hit.point, Quaternion.identity);
                Destroy(b, 1);
            }

            else if (hit.transform.gameObject.CompareTag("Wall"))
            {
                GameObject C = Instantiate(HitPoint2, hit.point, Quaternion.identity);
                Destroy(C, 1);
            }

            else if (hit.transform.gameObject.CompareTag("Floor"))
            {
                GameObject D = Instantiate(HitPoint3, hit.point, Quaternion.identity);
                Destroy(D, 1);
            }
            
            EnemyAI enemy = hit.transform.GetComponent<EnemyAI>();

            if (enemy != null)
            {
                enemy.TakeDamage(2);
            }
            
           
        }
    }
}


using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using JetBrains.Annotations;
using UnityEngine.Rendering.Universal.Internal;
//using Unity.UI;
using UnityEngine.UI;
using TMPro;

public class FPCONTROLLER : MonoBehaviour
{

    [Header("movement Settings")]
    public float moveSpeed = 5f;
    public float Gravity = -9.81f;
    public float jumpHeight = 1.5f;
    

    [Header("Look Settings")]
    public Transform cameraTransform;
    public float looksensitivity = 0.3f;
    public float verticalLookLimit = 90f;

    [Header("Shooting")]
   // public GameObject bulletprefab;
    public Transform gunpoint;
    public float bulletvelocity = 500;
    
    // for the particle system
    public GameObject fire;
    public GameObject HitPoint1;
    public GameObject HitPoint2;
    public GameObject HitPoint3;
    public GameObject Bullet;
    //new code


    [Header("Crouch")]
    public float crouchheight = 1f;
    public float standheight = 2f;
    public float crouchspeed = 2.5f;
    private float originalmovespeed;

    [Header("Pickup Settings")]
    public float pickupRange = 50f;
    public Transform holdPoint;
    private PickUpObject heldObject;

    [Header("Throw Settings")]
    public float throwForce = 10f;
    public float throwUpwardBoost = 1f;

    [Header("Door System / interacting with a object so that it changes color")]
    public float interactRange =25f;

    [Header("GameOver PopUp")]
   // public Text GameOver;
    public GameObject Panel;

    [Header("Health system")]
    private PlayerHealth HealthBar;
    private float maxHealth = 300f;
    public float currentHealth;


    [Header("Damage Screen")]
    public Image damageScreen;
    public float flashAlpha = 0.6f;
    public float fadeSpeed = 2f;
    private Color originalColor;

    private bool isDamaged = false;

    [Header("Character Movement / look settings")]
    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private float VerticalRotation = 0f;

    [Header("UI Controll")]
    public Canvas can;
    public Scrollbar healthBar;
    public TextMeshProUGUI healthText;


    private void Awake()
    {
        //can = GetComponent<Canvas>();
        //healthBar = GetComponent<Scrollbar>();
        this.HealthBar = this.GetComponentInChildren<PlayerHealth>();
        currentHealth = maxHealth;
        healthText.text = currentHealth.ToString();
        this.UpdateHealthBar();
        originalColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        
        // for the Ui Game over
        Panel.SetActive(false);
       // GameOver.gameObject.SetActive(true);
        
        
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        // for damage Screen
       

    }

    private void Start()
    {
        healthBar.value = currentHealth;
    }

    public void MattshealthBarUpdate()
    {
        healthBar.value = currentHealth;
        healthText.text = currentHealth.ToString();
    }

    // update function
    private void Update()
    {

        Handlemovement();
        HandleLook();

        /*if (heldObject !=null)
        {
            heldObject.MoveToHoldPoint(holdPoint.position);
        }*/

        if (!isDamaged)
        {
            damageScreen.color = Color.Lerp(damageScreen.color, new Color(originalColor.r, originalColor.g, originalColor.b, 0), fadeSpeed * Time.deltaTime);
        }
        else
        {
            isDamaged = false; // Reset after showing flash
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
        { 
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * Gravity); 
        }
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
                PickUpObject pickUp = hit.collider.GetComponentInParent<PickUpObject>();
                if (pickUp !=null)
                {
                    pickUp.PickUp(holdPoint);
                    heldObject = pickUp;

                }

            }
        }
        else
        { 
           heldObject.Drop();
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


    // Don't delete this code it's for future references.
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

    /*public void OnDoor(InputAction.CallbackContext context)
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
    */

    private  void Fire()
    {
        RaycastHit hit;
        if(Physics.Raycast(gunpoint.position , transform.TransformDirection(Vector3.forward) , out hit , 100))

        { Debug.DrawRay(gunpoint.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);


            GameObject a = Instantiate(fire, gunpoint.position, Quaternion.identity);

            Destroy(a, 1);

            Instantiate(Bullet, gunpoint.transform.position, gunpoint.transform.rotation);

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
                enemy.TakeDamage(1);
            }
           
            
           
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        this.UpdateHealthBar();
        // Show the damage flash
        damageScreen.color = new Color(originalColor.r, originalColor.g, originalColor.b, flashAlpha);
        isDamaged = true;
        MattshealthBarUpdate();

        if (currentHealth <= 0)
        {
            // Invoke(nameof(DestroyPlayer), 0.5f);
            currentHealth = 0;
            Time.timeScale = 0f;
            Panel.SetActive(true);
            //GameOver.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

    }

    private void DestroyPlayer()
    {
        Destroy(gameObject);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            KeyItem Key = hit.collider.GetComponentInParent<KeyItem>();

            if (Key != null)
            {
                Key.Collect();
                return;
            }


            WinningItem Win = hit.collider.GetComponent<WinningItem>();
            if (Win != null)
            {
                Win.collect();
                return;
            }

            DoorInteraction door = hit.collider.GetComponent<DoorInteraction>();

            if (door != null)
            {
                door.TryOpen();
                return;
               // door.ToggleDoor();
            }

           
        }
    }

    private void UpdateHealthBar()
    {
        float percentHealth = this.currentHealth/ this.maxHealth;
        this.HealthBar.UpdateHealthBarAmount(percentHealth);
    }
}

 
using UnityEngine;
using UnityEngine.InputSystem;
public class fpscontroller : MonoBehaviour
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

 // update function
    void Update()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

    }


}


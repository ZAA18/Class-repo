using UnityEngine;

public class RotatingDoorStayOpen : MonoBehaviour
{
    public Transform door; // Assign the door model (can be self)
    public float openAngle = 90f; // How much it rotates when open
    public float rotationSpeed = 3f; // Speed of rotation

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool isOpen = false; // Track if door has opened

    void Start()
    {
        if (door == null) door = transform;

        closedRotation = door.rotation;
        openRotation = closedRotation * Quaternion.Euler(0, 0, openAngle);
    }

    void Update()
    {
        // Smoothly rotate towards the current target rotation
        if (isOpen)
        {
            door.rotation = Quaternion.Slerp(door.rotation, openRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isOpen && other.CompareTag("Player"))
        {
            isOpen = true; // Open the door and keep it open
        }
    }
}


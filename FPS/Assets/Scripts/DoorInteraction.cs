using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class DoorInteraction : MonoBehaviour
{
    public float OpenAngle = 90f;
    public float OpenSpeed = 2f;
    public bool isOpen = false;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Coroutine currentCoroutine;

    [Header("Lock Settings")]
    public bool requiresKey = false;  // This is for the safe or other doors that might require a lock
    public string KeyName = "AccessCard";

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, OpenAngle, 0));
    }

    public void TryOpen()
    { 
      if (requiresKey)
      {
            if (!InventorySystem.HasKey(KeyName))
            {
                Debug.Log("This door is locked. You need a access card");
               
                return;

            }
           
        }
        // Now you can open the door
        ToggleDoor();

    }
    public void ToggleDoor()
    {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(ToggleRoutine());
    }

    private IEnumerator ToggleRoutine()
    {
        Quaternion targetRotation = isOpen ? closedRotation : openRotation;
        isOpen = !isOpen;

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.01f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * OpenSpeed);
            yield return null;
        }

        transform.rotation = targetRotation;
    }
} 
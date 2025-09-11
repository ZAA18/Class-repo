using System.Collections;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public float OpenAngle = 90f;
    public float OpenSpeed = 2f;
    public bool isOpen = false;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Coroutine currentCoroutine;

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, OpenAngle, 0));
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
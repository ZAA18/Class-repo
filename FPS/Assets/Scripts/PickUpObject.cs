using UnityEngine;

public class PickUpObject : MonoBehaviour
{

    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody>();   
    }

    

    public void PickUp( Transform holdPoint)
    {

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.useGravity = false;
            rb.isKinematic = true; // prevent the physics from fighting
        }


        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero; //Snap to hold point
        transform.localRotation = Quaternion.identity;

        // Make sure the holdPoint scale is (1 - 1 -1 ) in the inspector to avoid warping.
        transform.localScale = Vector3.one; // prevents warping

    }

    public void Drop()
    {
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        transform.SetParent(null);
        transform.localScale = Vector3.one;
    }

    public void MoveToHoldPoint(Vector3 targetPosition)
    {
        if (rb != null)

            rb.MovePosition(targetPosition);
        else
            transform.position = targetPosition;
    }
}

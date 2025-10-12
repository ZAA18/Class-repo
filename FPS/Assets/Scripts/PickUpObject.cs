using UnityEngine;

public class PickUpObject : MonoBehaviour
{

    private Rigidbody rb;
    private Vector3 originalWorldScale;

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

        originalWorldScale = transform.lossyScale;
        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero; //Snap to hold point
        transform.localRotation = Quaternion.identity;

        // Make sure the holdPoint scale is (1 - 1 -1 ) in the inspector to avoid warping.
        // transform.localScale = Vector3.one; // prevents warping

        //Restore the oroginal world Scale

        transform.localScale = Vector3.one;
        transform.localScale = new Vector3(originalWorldScale.x / transform.lossyScale.x, originalWorldScale.y / transform.lossyScale.y, originalWorldScale.z / transform.lossyScale.z);

    }

    public void Drop()
    {
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        originalWorldScale = transform.lossyScale;
        transform.SetParent(null);
        transform.localScale = originalWorldScale;
    }

 /*   public void MoveToHoldPoint(Vector3 targetPosition)
    {
        if (rb != null)

            rb.MovePosition(targetPosition);
        else
            transform.position = targetPosition;
    }*/
}

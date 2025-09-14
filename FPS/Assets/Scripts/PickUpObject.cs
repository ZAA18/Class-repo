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
        rb.useGravity = false;
        rb.isKinematic = true; // prevent the physics from fighting
       rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero; //Snap to hold point
        transform.localRotation = Quaternion.identity;


    }

    public void Drop()
    {
        rb.useGravity = true;
        rb.isKinematic = false;
        transform.SetParent(null);
    }

    public void MoveToHoldPoint(Vector3 targetPosition)
    { rb.MovePosition(targetPosition); }
}

using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public bool isPickedUp = false;
    public Vector3 offset = new Vector3(0, 1, 1); // Position relative to player

    public virtual void PickUp(Transform playerTransform)
    {
        isPickedUp = true;

        // Parent to player
        transform.SetParent(playerTransform);
        transform.localPosition = offset;
        transform.localRotation = Quaternion.identity;
        transform.localScale = new Vector3(1f, 1f, 1f);

        // Rigidbody
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        // Collider
        Collider col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;
    }

    public virtual void HeldUpdate() { }

    void Update()
    {
        if (isPickedUp) HeldUpdate();
    }
}


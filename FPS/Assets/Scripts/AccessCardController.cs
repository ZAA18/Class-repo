using UnityEngine;
using UnityEngine.UI;

public class AccessCardController : MonoBehaviour
{
    public Text pickupText;
    public float displayDistance = -10f;
    public Vector3 offset = new Vector3(0, 1f, 1f);

    [HideInInspector] public bool isPickedUp = false;
    private Transform player;

    void Start()
    {
        if (pickupText != null) pickupText.gameObject.SetActive(false);

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void Update()
    {
        if (isPickedUp) return; // don't show text if already picked up
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (pickupText != null) pickupText.gameObject.SetActive(distance <= displayDistance);
    }

    public void PickUp(Transform playerTransform)
    {
        if (isPickedUp) return;
        isPickedUp = true;

        transform.SetParent(playerTransform);
        transform.localPosition = offset;
        transform.localRotation = Quaternion.identity;
        transform.localScale = new Vector3(1f, 1f, 1f);

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        Collider col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;

        if (pickupText != null) pickupText.gameObject.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isPickedUp) return;
        if (other.CompareTag("Vault"))
        {
            BankVault vault = other.GetComponent<BankVault>();
            if (vault != null && !vault.Moving) vault.Moving = true;
        }
    }
}
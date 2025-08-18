using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public float pickupRange = -5f;
    public KeyCode pickupKey = KeyCode.E;
    private AccessCardController heldCard;

    void Update()
    {
        if (heldCard == null && Input.GetKeyDown(pickupKey))
        {
            TryPickUpCard();
        }
    }

    void TryPickUpCard()
    {
        AccessCardController[] cards = FindObjectsOfType<AccessCardController>();
        foreach (var card in cards)
        {
            if (card.isPickedUp) continue;

            float distance = Vector3.Distance(transform.position, card.transform.position);
            if (distance <= pickupRange)
            {
                card.PickUp(transform);
                heldCard = card;
                Debug.Log("Picked up: " + card.name);
                break;
            }
        }
    }
}


using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Collider))]
public class HealablePickup : MonoBehaviour
{
    public enum HealMode { AddAmount, FullRestore }

    [Header("Pickup Settings")]
    public HealMode healMode = HealMode.AddAmount;
    public float healAmount = 30f;
    [Range(0f, 1f)]
    public float rquireBelowPercent = 0.999f;
    public string playerTag = "Player";

    [Header("Behaviour")]
    public bool destroyOnPickup = true;
    [Header("Sound for PickUp")]
    public UnityEvent onPicked;

    private void Reset()
    {
        var col = GetComponent<Collider>();
        if (col == null)
            col = gameObject.AddComponent<BoxCollider>();
        col.isTrigger = true;
    }

    private void Awake()
    {
        var col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag))
            return;

        FPCONTROLLER fp = other.GetComponent<FPCONTROLLER>();

        if (fp == null) 
            
        fp = other.GetComponentInParent<FPCONTROLLER>();

        if (fp == null)
        {
            Debug.LogWarning($"HealablePickUp: no FPCONTROLLER Found on Object {other.name}");
            return;
        }

        ApplyPickupTo(fp);

    }

    private void ApplyPickupTo(FPCONTROLLER fp)
    {
        if (healMode == HealMode.AddAmount)
            fp.Heal(healAmount);
        else
            fp.RestoreToFull();

        onPicked?.Invoke();

        if (destroyOnPickup)
            Destroy(gameObject);
        else gameObject.SetActive(false);
    }
}

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.Rendering;


[RequireComponent(typeof(Collider))]
public class HealablePickup : MonoBehaviour
{
    public enum HealMode { AddAmount, FullRestore }

    [Header("HealPickup Settings")]
    public HealMode healMode = HealMode.AddAmount;
    public float healAmount = 30f;
    [Range(0f, 1f)]
    public float requireBelowPercent = 0.999f;
    public string playerTag = "Player";

    [Header("Behaviour")]
    public bool destroyOnPickup = true;
    [Header("Sound for PickUp")]
    public UnityEvent onPicked;

   public void PickupBy(FPCONTROLLER fp)
    {
        if (fp == null)
        {
            Debug.LogWarning("HealablePickup.PickupBy: fp is null");
            return;
        }

    
        Debug.Log($"HealablePickup. Player tried to up {name} (mode = {healMode}, amount = {healAmount})");

        if (healMode == HealMode.AddAmount)
        {
            float current = fp.currentHealth;
            float max = fp.MaxHealth;
            float percent = current / max;

            Debug.Log($"HealablePickup: Player health {current} ({percent:P0}), requireBelowPercent = {requireBelowPercent:P0}");


            if (percent < requireBelowPercent)
            {
                fp.Heal(healAmount);
                Debug.Log($"HealablePickup: Applied {healAmount} HP to player.");

            }
            else
            {
                fp.StoreHealable(healMode, healAmount);
                Debug.Log($"HealablePickup: Player health too high - stored healable in inventory.");
            }
        }

        else
        {
            fp.StoreHealable(healMode, healAmount);
            Debug.Log("HealablePickup: Full packed stored for later");

        }

        onPicked?.Invoke();

        if (destroyOnPickup)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);

        
    
    }

}

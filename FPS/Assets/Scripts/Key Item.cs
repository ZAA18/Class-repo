using UnityEngine;

public class KeyItem : MonoBehaviour
{
    public string KeyName = "AccessCard";
   
    public void Collect()
    {
        InventorySystem.AddKey(KeyName);

        //Destroy the access card from the world (prevents duplicate pickups)
        Debug.Log("KeyItem: Collected'" + KeyName + "'");
        Destroy(gameObject);
    }

}

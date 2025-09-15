using UnityEngine;

public class KeyItem : MonoBehaviour
{
    public string KeyName = "AccessCard";
   
    public void Collect()
    {
        InventorySystem.AddKey(KeyName);
        Destroy(gameObject);
    }
}

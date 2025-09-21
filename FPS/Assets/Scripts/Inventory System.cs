using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    private static HashSet<string> Keys = new HashSet<string>();

    public static void AddKey(string KeyName)
    {
        Keys.Add(KeyName);
        Debug.Log("Inventory: Added Key " + KeyName + "");
    }

    public static bool HasKey(string KeyName)
    {
        bool has = Keys.Contains(KeyName);
        Debug.Log("Inventory: HasKey('" + KeyName + "') ->" + has);
        
        return has;
    
    }

    public static void ClearAll()
    {
        Keys.Clear();
        Debug.Log("Inventory: Cleared All Keys");
      
    }
}

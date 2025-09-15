using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    private static HashSet<string> Keys = new HashSet<string>();

    public static void AddKey(string KeyName)
    {
        Keys.Add(KeyName);
        Debug.Log("Picked Up: " + KeyName);
    }

    public static bool HasKey(string KeyName)
    {
        return Keys.Contains(KeyName);
    
    }
}

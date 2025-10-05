using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.ProBuilder.MeshOperations;


public class WeaponSwitching : MonoBehaviour
{
    public int selectedWeapons = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        selectedWeapon();
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void OnWeapon(InputAction.CallbackContext context)
    {
        int previousSelectedWeapon = selectedWeapons;
        
        if (context.performed)
        {  if (selectedWeapons >= transform.childCount - 1)
                selectedWeapons = 0;
            else
                selectedWeapons++;
        if (previousSelectedWeapon != selectedWeapons)
            {
                selectedWeapon();
            }
       
        }
    }

   public void selectedWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapons)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
                i++;

        }
    }
}

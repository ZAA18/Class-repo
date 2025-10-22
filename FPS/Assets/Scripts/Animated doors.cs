using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;

public class Animateddoors : MonoBehaviour
{
    [Header("DoorAnimation")]
    public Animator doorAnimator;

    //for the access card
    public bool requiresKey = false;

    public string KeyName = "AccessCard";

    public bool isOpen = false;

    private bool isAnimating = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Awake()
    {

        if (doorAnimator == null)
            doorAnimator = GetComponentInChildren<Animator>();

    
    }

    public void TryOpen()
    {

        if (isAnimating)
        {
            Debug.Log("AnimatedDoor: Door is currently animating, wait until finished.");
            return;

        }

        if (requiresKey && !InventorySystem.HasKey(KeyName))
        {
            Debug.Log($"AnimatedDoor : Door is Locked. Requires : {KeyName}");
            return;
        }
        ToggleDoor();

    }

      private void ToggleDoor()
        {
            isOpen = !isOpen;
            StartCoroutine(AnimateDoor());
        }

        private IEnumerator AnimateDoor()
        {
            isAnimating = true;

            if (doorAnimator != null)
            {
                doorAnimator.SetBool("isOpen", isOpen);
            }

            else
            {
                Debug.LogWarning($"Animateddoors'{gameObject.name}' has no Animator assigned!");

            }
            yield return new WaitForSeconds(1f);

            isAnimating = false;
        }
}

    
        
    

    


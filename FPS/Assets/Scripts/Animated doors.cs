using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;

public class Animateddoors : MonoBehaviour
{
    [Header("DoorAnimation")]
    public Animator doorAnimator;
    public string openTrigger = "Open";
    public string closeTrigger = "Close";
    public string openBool = "isOpen";
    public float animationDuration = 1.2f;

    //for the access card
    public bool requiresKey = false;

    public string KeyName = "AccessCard";

    public bool isOpen = false;

    private bool isAnimating = false;

    [Header("Audio Settings (Optional)")]
    public AudioSource doorAudio;
    public AudioClip openSound;
    public AudioClip lockedSound;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Awake()
    {

        if (doorAnimator == null)
            doorAnimator = GetComponentInChildren<Animator>();

        if (doorAudio == null)
            doorAudio = GetComponent<AudioSource>();
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
            if (doorAnimator.HasParameterOfType(openTrigger, AnimatorControllerParameterType.Trigger))
            {
                doorAnimator.SetTrigger(isOpen ? openTrigger : closeTrigger);
            }

            else if (doorAnimator.HasParameterOfType(openBool, AnimatorControllerParameterType.Bool))
            {
                doorAnimator.SetBool(openBool, isOpen);

            }

        }
        else

        {
            Debug.LogWarning($"AnimatedDoor '{gameObject.name}'has no Animator assigned!");

        }
        yield return new WaitForSeconds(1f);

        isAnimating = false;
    }


    private void playOpenSound()
    {
        if (doorAudio != null && openSound != null)
            doorAudio.PlayOneShot(openSound);
    }

    private void PlayLockedSound()
    {
        if (doorAudio != null && lockedSound != null)
            doorAudio.PlayOneShot(lockedSound);
    }
}

       

          

    
        
    

    


using UnityEngine;

public class MenuAnimationController : MonoBehaviour
{
    public Animator armatureAnimator;
    public Animator gunAnimator;

    void Start()
    {
        // Play both at the same time
        armatureAnimator.Play("ArmatureMenu");
        gunAnimator.Play("GunFalling");
    }
}
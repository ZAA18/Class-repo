using UnityEngine;

public class Mouse : MonoBehaviour
{  // this is for the mouse sensitivity
    public float mouseSensitivity = 100f;
    //
    float XROTATION = 0;
    float YROTATION = 0;
    // did this for efficiency and testing 
    public float topclamp = -90f;
    public float bottomclamp = 90f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        // the reason for this is that when our player rotates the mouse x=y and y =x // its like locking the mouse to focus on one axis.
        // += is use for flight simulator those complicated driving games.
        XROTATION -= mouseY;

        // clamp the rotation  // we are action blocking the mouse rotation
        XROTATION = Mathf.Clamp(XROTATION, topclamp, bottomclamp);

        YROTATION -= mouseX;

        transform.localRotation = Quaternion.Euler(XROTATION, YROTATION, 0f);

    }
}

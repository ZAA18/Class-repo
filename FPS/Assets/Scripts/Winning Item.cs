using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class WinningItem : MonoBehaviour
{
    [Header("Win Settings")]
    public string WinSceneName = "WinScene"; // load scene
    public int pointsOnPickup = 100;
    public bool destroyOnpickup = true;

    public void collect()
    {
        Debug.Log("You got the money! You Win!");

        if (destroyOnpickup)
        { Destroy(gameObject); }

        Time.timeScale = 0;
    }
}

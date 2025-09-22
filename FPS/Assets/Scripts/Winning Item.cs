using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class WinningItem : MonoBehaviour
{
    [Header("Win Settings")]
    public string WinSceneName = "WinScene"; // load scene
    public int pointsOnPickup = 100;
    public bool destroyOnpickup = true;
    public GameObject winPanel;

    public void Awake()
    {
        winPanel.SetActive(false);
    }
    public void collect()
    {
        Debug.Log("You got the money! You Win!");
        if (winPanel != null)
        {
            winPanel.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (destroyOnpickup)
        { gameObject.SetActive(false); }

       // Time.timeScale = 0;
    }

    public void Retry()
    { Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);   
    }
}

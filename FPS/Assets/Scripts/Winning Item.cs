using UnityEngine;
using UnityEngine.SceneManagement;

public class WinningItem : MonoBehaviour
{
    public string WinSceneName = "WinScene";

    public void collect()
    {
        Debug.Log("You got the money! You Win!");
       // SceneManager.LoadScene(WinSceneName);
    }
}

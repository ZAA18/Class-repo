using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("UI Panels")]

    public GameObject mainMenuPanel;
    public GameObject instructionsPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    
    public void LoadSceneByIndex(int index)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(index);
    }

    public void LoadSceneByName(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);

    }

    public void PlayGame(int sceneIndex)
    {
        LoadSceneByIndex(sceneIndex);

    }

    public void Showinstructions()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);
        if (instructionsPanel != null)
            instructionsPanel.SetActive(true);

    }

    public void HideInstructions()
    {
        if (instructionsPanel != null)
            instructionsPanel.SetActive(false);

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
    }
        
    public void ExitGame()
    {
        Debug.Log("exiting game...");
        Application.Quit();
    }

    public void BackToMenu()
    {
        LoadSceneByIndex(0);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

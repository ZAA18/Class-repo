using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
//using UnityEditor.Search;

public class StoryManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text StoryText;
    public Image StoryImage;
    public GameObject ContinueButton;

    [Header("Story Settings")]
    [TextArea(3, 5)]
    public string[] StoryLines;
    public Sprite[] StoryPictures;
    public float typingSpeed = 7f;

    private int currentIndex = 0;
    private bool isTyping = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StoryText.text = "";
        ContinueButton.SetActive(false);
        ShowLine();

        
    }


    void ShowLine()
    {
        StoryText.text = "";
        ContinueButton.SetActive(false);

        if (currentIndex < StoryLines.Length)
        {
            if (currentIndex < StoryPictures.Length)
            {
                StoryImage.sprite = StoryPictures[currentIndex];
            }

            StartCoroutine(TypeLine(StoryLines[currentIndex]));
        }

        else
        {
            SceneManager.LoadScene(2);
        }
        
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        foreach (char c in line.ToCharArray())
        {
            StoryText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        ContinueButton.SetActive(true);
    }

    public void OnContinue()
    {
        if (isTyping)
        {
            // to skip typing and instantly show full line
            StopAllCoroutines();
            StoryText.text = StoryLines[currentIndex];
            isTyping = false;
            ContinueButton.SetActive(true);
        }

        else

        {
            currentIndex++;
            ShowLine();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

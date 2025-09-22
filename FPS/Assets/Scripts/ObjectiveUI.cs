using TMPro;
using UnityEngine;

public class ObjectiveUI : MonoBehaviour
{
    [Header("Objective Texts")]
    public TextMeshProUGUI objective1;
    public TextMeshProUGUI objective2;
    public TextMeshProUGUI objective3;
    public TextMeshProUGUI objective4;

    private int currentObjective = 1;

   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objective1.color = Color.white;
        objective2.color = Color.gray;
        objective3.color = Color.gray;
        objective4.color = Color.gray;
    }

    public void CompleteObjective()
    {
        if (currentObjective == 1)
        {
            objective1.text += " Completed";
            objective1.color = Color.green;
            objective2.color = Color.white;
            currentObjective++;
        }
        else if (currentObjective == 2)
        {
            objective2.text += " Completed";
            objective2.color = Color.green;
            objective3.color = Color.white;
            currentObjective++;
        }
        else if (currentObjective == 3)
        {
            objective3.text += " Completed";
            objective3.color = Color.green;
            objective4.color = Color.white;
            currentObjective++;
        }
        else if (currentObjective == 4)
        {
            objective4.text += " Completed";
            objective4.color = Color.green;

            // All done → show win UI
          //  FindObjectOfType<WinUI>().ShowWin();
        }
    }
}

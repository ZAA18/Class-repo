using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private Image HealthImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void UpdateHealthBarAmount(float percentHealth)
    {
        this.HealthImage.fillAmount = percentHealth;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

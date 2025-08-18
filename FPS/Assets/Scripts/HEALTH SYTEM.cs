using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using System.Collections.Generic;
using Image = UnityEngine.UI.Image;

public class HEALTHSYTEM : MonoBehaviour
{
    [SerializeField] private Image healthbar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatehealthBar(float maxhealth, float currentHealth)
    { healthbar.fillAmount = (currentHealth / maxhealth); }
}

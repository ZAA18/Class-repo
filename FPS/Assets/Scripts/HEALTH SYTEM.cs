using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using System.Collections.Generic;
using Image = UnityEngine.UI.Image;


public class HEALTHSYTEM : MonoBehaviour
{
    [SerializeField] private Image healthbar;

    // private Camera cam;

    [Header("health Settings")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    private float HealthCheck;

    [Header("UI SETTINGS")]
    [SerializeField] private UnityEngine.UI.Slider healthbar2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        //cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
        Debug.Log("the currenthealth on the health system script is" + HealthCheck);
    }

    public void UpdatehealthBar(float maxhealth, float currentHealth)
    { //healthbar.fillAmount = (currentHealth / maxhealth);
        HealthCheck = (currentHealth / maxhealth);
    }
}

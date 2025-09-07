using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Bullet : MonoBehaviour
{

    [SerializeField] private float maxhealth = 50;

    private float currentHealth;

    [SerializeField] private HEALTHSYTEM HEALTHSYTEM;
    public float healthcheck = 100f;
    public float damage = 0.8f;
    public float realhealth;


    public void Start()
    {
        currentHealth = maxhealth;
       // HEALTHSYTEM.UpdatehealthBar(maxhealth, currentHealth);
    }

    public void Update()
    {
        Debug.Log("This is the currenthealth in the bullet script" + currentHealth);
       // HEALTHSYTEM.UpdatehealthBar(maxhealth, currentHealth);
        Debug.Log("Health test" + healthcheck);
        Debug.Log("Real health update" + realhealth);
    }

    private void OnCollisionEnter(Collision collision)
    { 
        if (collision.gameObject.CompareTag("Player"))
        {
            print("hit" + collision.gameObject.name + "!");
            currentHealth -= Random.Range(0.5f, 1.5f);
            Debug.Log("the Health now is :" + currentHealth);
             realhealth -=healthcheck - damage;
            Debug.Log("the real health is" + realhealth);
            Debug.Log("the health is now" + healthcheck);
            Destroy(gameObject);


           // HEALTHSYTEM.UpdatehealthBar(maxhealth, currentHealth);
        }

        if (collision.gameObject.CompareTag ("wall"))
        {
            print("hit the wall" + collision.gameObject.name + "!");
            Destroy(gameObject);

        }
    }
}


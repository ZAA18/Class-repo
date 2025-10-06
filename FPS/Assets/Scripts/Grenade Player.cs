using System.Threading;
using UnityEngine;

public class GrenadePlayer : MonoBehaviour
{
    public float delay = 3f;
    float countdown;
    bool hasExploded = false;

    //particle sytem

    public GameObject explosionEffect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {

        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExploded) 
        {
            Explode();
            hasExploded = true;
        }

    }

    public void Explode ()
    {
        Debug.Log("Boom");

        Instantiate(explosionEffect, transform.position, transform.rotation);
        //Show effect 

        //Get nerby objects 
        // add force
        // Damage

        // Remove grenade
    }
}

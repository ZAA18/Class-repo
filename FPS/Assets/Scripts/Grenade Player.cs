using System.Threading;
using JetBrains.Annotations;
using UnityEngine;

public class GrenadePlayer : MonoBehaviour
{
    public float delay = 3f;
    float countdown;
    bool hasExploded = false;
    public float radius = 5f;
    public float force = 700f;
    //particle sytem
    public GameObject explosionEffect;
   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        countdown = delay;

    }

    // Update is called once per frame
    public void start()
    {

        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExploded)
        {
            Explode();
            //explosionEffect.Play();
            hasExploded = true;
        }

    }

    public void Explode()
    {
        Debug.Log("Boom");

        Instantiate(explosionEffect, transform.position, transform.rotation);

        //Show effect 

        //Get nerby objects 

        Collider[] collidersToDestroy = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObject in collidersToDestroy)
        {
            //adding force
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();

            if (rb != null)
            { rb.AddExplosionForce(force, transform.position, radius); }


            Destructible dest = nearbyObject.GetComponent<Destructible>();
            if (dest != null)
            {
                dest.Destroy();
            }
        }

        Collider[] collidersToMove = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObject in collidersToMove)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();

            if (rb != null)
            { rb.AddExplosionForce(force, transform.position, radius); }

            // add force

            // Damage

            // Remove grenade
        }
    }
}

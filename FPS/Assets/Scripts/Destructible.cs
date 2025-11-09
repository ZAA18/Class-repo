using UnityEngine;
//using System.Collections.Generic;
//using System.Collections;

public class Destructible : MonoBehaviour
{
    public GameObject destroyedVersion;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Destroy()
    {
        Instantiate(destroyedVersion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}

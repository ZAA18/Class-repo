using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            print("hit" + collision.gameObject.name + "!");
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag ("wall"))
        {
            print("hit the wall" + collision.gameObject.name + "!");
            Destroy(gameObject);
        }
    }
}

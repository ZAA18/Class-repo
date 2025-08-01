using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class Gun : MonoBehaviour


{

    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30f;
    public float bulletPrefablifeTime = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
        // left mouse click
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            FireWeapon();
        }
    }

    private void FireWeapon()
    {
        // Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        
        //Shoot the bullet
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward.normalized * bulletVelocity, ForceMode.Impulse);
        // Destroy the bullet after some time
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefablifeTime));
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float bulletPrefablifeTime)
    { yield return new WaitForSeconds(3f);
        Destroy(bullet);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
//using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class GunPlayer : MonoBehaviour
{

    [Header("Shooting")]

    public Transform gunpoint;
    public float bulletvelocity = 500;
    public float bulletrange = 200f;
    public float NextTimeToFire = 0f;
    public float fireRate = 15f;

    //ammo
    public int maxAmmo = 10;
    private int currentAmmo;
    public float reloadTime = 5f;
    private bool isReloading = false;
    private bool isShooting = false;

    //animation

    public Animator animator;

    // for the particle system
    public GameObject fire;
    public GameObject HitPoint1;
    public GameObject HitPoint2;
    public GameObject HitPoint3;
    public GameObject Bullet;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {

        if (currentAmmo == -1)
            currentAmmo = maxAmmo;

    }

    public void OnEnable()
    {
        isReloading = false;
        isShooting = false;
        animator.SetBool("Reloading", false);
        animator.SetBool("Shooting", false);
    }

    // Update is called once per frame
    void Update()
    {
       



        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading..");

        animator.SetBool("Reloading", true);

        yield return new WaitForSeconds(reloadTime);

        animator.SetBool("Reloading", false);

        currentAmmo = maxAmmo;
        isReloading = false;
    }

    IEnumerator Attacking ()
    {
        isShooting = true;
        Debug.Log("Attacking");
        animator.SetBool("Shooting", true);
        yield return new WaitForSeconds(1);
        animator.SetBool("Shooting", false);
        isShooting = false;
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (isReloading)
            return;

        if (isShooting)
            return;
        {
            
        }

        if (context.performed && Time.time >= NextTimeToFire)
        {
            NextTimeToFire = Time.time + 1f / fireRate;
            Fire();
            StartCoroutine(Attacking());
            return;
        }
    }

    private void Fire()
    {
        currentAmmo--;
        RaycastHit hit;
        if (Physics.Raycast(gunpoint.position, transform.TransformDirection(Vector3.forward), out hit, bulletrange))

        {
            Debug.DrawRay(gunpoint.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log(hit.transform.name);

            GameObject a = Instantiate(fire, gunpoint.position, Quaternion.identity);

            Destroy(a, 0.5f);

            Instantiate(Bullet, gunpoint.transform.position, gunpoint.transform.rotation);

            if (hit.transform.gameObject.CompareTag("Target"))
            {
                GameObject b = Instantiate(HitPoint1, hit.point, Quaternion.identity);
                Destroy(b, 0.5f);
            }

            else if (hit.transform.gameObject.CompareTag("Wall"))
            {
                GameObject C = Instantiate(HitPoint2, hit.point, Quaternion.identity);
                Destroy(C, 0.5f);
            }

            else if (hit.transform.gameObject.CompareTag("Floor"))
            {
                GameObject D = Instantiate(HitPoint3, hit.point, Quaternion.identity);
                Destroy(D, 0.5f);
            }

            EnemyAI enemy = hit.transform.GetComponent<EnemyAI>();

            if (enemy != null)
            {
                enemy.TakeDamage(1);
            }



        }
    }
}

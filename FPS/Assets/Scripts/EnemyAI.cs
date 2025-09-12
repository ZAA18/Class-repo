//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{


    public NavMeshAgent agent;
    public Transform player;
    public LayerMask WhatisGround, whatisPlayer;


    [Header("Patrol Waypoints")]
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;

    [Header("Attacking")]
    public float TimebetweenAttacks = 5f;
    bool alreadyAttacked;
    public GameObject projitile;
    public Transform gunpoint;
    public GameObject fire;
    public GameObject HitPoint1;
    public GameObject HitPoint2;
    public GameObject HitPoint3;  


    [Header("States")]
    public float sightrange, attackrange;
    public bool playerInSightRange, playerinAttackRange;


    [Header("Health system")]
    private PlayerHealth HealthBar;
    private float maxHealth = 10f;
    float currentHealth;

    [Header("Trying to implement bullet system")]
    public GameObject bulletPrefab;
    Rigidbody rb;




    public void Awake()
    {
        this.HealthBar = this.GetComponentInChildren<PlayerHealth>();
        currentHealth = maxHealth;
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()

    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightrange, whatisPlayer);
        playerinAttackRange = Physics.CheckSphere(transform.position, attackrange, whatisPlayer);

        if (!playerInSightRange && !playerinAttackRange)
            Patroling();
        else if (playerInSightRange && !playerinAttackRange)
            ChasePlayer();
        else if (playerinAttackRange && playerInSightRange)
            AttackPlayer();
            Debug.Log("Attacking Player");
    }
    // the function below patrol supports patrol
    private void Patroling()
    {
        if (waypoints.Length == 0) return;

        agent.SetDestination(waypoints[currentWaypointIndex].position);

        // Check if enemy reached waypoint
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }


    }

 

    private void ChasePlayer()
    { agent.SetDestination(player.position); }

    /* private void AttackPlayer()
     { 

         //Make sure enemy does not move
         agent.SetDestination(transform.position);

         transform.LookAt(player); // did this because i want the nav to control the rotation of the player, its a bit faster now

         if (!alreadyAttacked)

         {
             Invoke(nameof(ResetAttack), TimebetweenAttacks);
           //  DestroyBullet();
             rb = Instantiate(projitile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
             rb.AddForce(transform.forward * 50f, ForceMode.Impulse);
           //  rb.AddForce(transform.up * 8f, ForceMode.Impulse);
         }
     }
    */

    private void AttackPlayer()
    {
        // Stop moving
        agent.SetDestination(transform.position);

        // Rotate towards player
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // Attack cooldown
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), TimebetweenAttacks);

            // 🔹 Raycast for hit detection
            RaycastHit hit;
            if (Physics.Raycast(gunpoint.position, transform.forward, out hit, 100))
            {
                Debug.DrawRay(gunpoint.position, transform.forward * hit.distance, Color.yellow);

                // Muzzle flash
                GameObject a = Instantiate(fire, gunpoint.position, Quaternion.identity);
                Destroy(a, 1);

                // Hit effects depending on tag
                if (hit.transform.CompareTag("Player"))
                {
                    GameObject b = Instantiate(HitPoint1, hit.point, Quaternion.identity);
                    Destroy(b, 1);
                }
                else if (hit.transform.CompareTag("Wall"))
                {
                    GameObject c = Instantiate(HitPoint2, hit.point, Quaternion.identity);
                    Destroy(c, 1);
                }
                else if (hit.transform.CompareTag("Floor"))
                {
                    GameObject d = Instantiate(HitPoint3, hit.point, Quaternion.identity);
                    Destroy(d, 1);
                }

                // Apply damage if the target has a FPCONTROLLER
                FPCONTROLLER user = hit.transform.GetComponent<FPCONTROLLER>();
                if (user != null)
                {
                    user.TakeDamage(2);
                }
            }
        }
    }

    public void ResetAttack()
    { alreadyAttacked = false;
       // Destroy(rb);
        
    }

    public void TakeDamage(int damage)
    {
        this.UpdateHealthBar();
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Invoke(nameof(DestroyEnemy), 0f);
        }
    }

    private void DestroyEnemy()
    { Destroy(gameObject);
    }


    private void UpdateHealthBar()
    {
        float percentHealth = this.currentHealth / this.maxHealth;
        this.HealthBar.UpdateHealthBarAmount(percentHealth);
    }

   

    /*private void DestroyBullet()
    {
        Destroy(projitile);
    }

    /*private void onDrwGizmosselected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackrange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightrange);
    }
    */
}

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
    public float TimebetweenAttacks = 1.5f;
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


    [Header("States")]
    private float currentHealth = 10f;

    [Header("Trying to implement bullet system")]
    public GameObject bulletPrefab;
    Rigidbody rb;




    public void Awake()
    {
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

        agent.SetDestination(transform.position);
        transform.LookAt(player);
        RaycastHit hit;
        
        if (Physics.Raycast(gunpoint.position, transform.TransformDirection(Vector3.forward), out hit, 100))

        {
            Debug.DrawRay(gunpoint.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);


            GameObject a = Instantiate(fire, gunpoint.position, Quaternion.identity);

            Destroy(a, 1);

            if (hit.transform.gameObject.CompareTag("Player"))
            {
                GameObject b = Instantiate(HitPoint1, hit.point, Quaternion.identity);
                Destroy(b, 1);
            }

            else if (hit.transform.gameObject.CompareTag("Wall"))
            {
                GameObject C = Instantiate(HitPoint2, hit.point, Quaternion.identity);
                Destroy(C, 1);
            }

            else if (hit.transform.gameObject.CompareTag("Floor"))
            {
                GameObject D = Instantiate(HitPoint3, hit.point, Quaternion.identity);
                Destroy(D, 1);
            }

            FPCONTROLLER User = hit.transform.GetComponent<FPCONTROLLER>();

            if (User != null)
            {
                User.TakeDamage(2);
            }
        }
    

        }

    public void ResetAttack()
    { alreadyAttacked = false;
       // Destroy(rb);
        
    }

    public void TakeDamage(int damage)
    { currentHealth -= damage;

        if (currentHealth <= 0)
            Invoke(nameof(DestroyEnemy), 0.5f);

    }

    private void DestroyEnemy()
    { Destroy(gameObject);
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

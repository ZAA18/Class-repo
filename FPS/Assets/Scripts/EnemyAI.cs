using UnityEditor.ShaderGraph.Internal;
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
    public float TimebetweenAttacks;
    bool alreadyAttacked;
    public GameObject projitile;

    
    [Header ("States")]
    public float sightrange, attackrange;
    public bool playerInSightRange, playerinAttackRange;

    
    [Header ("States")]
    public float health;

   


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

    private void AttackPlayer()
    { 
        
        //Make sure enemy does not move
        agent.SetDestination(transform.position);

       // transform.LookAt(player); // did this because i want the nav to control the rotation of the player, its a bit faster now

        if (!alreadyAttacked)

        {
            Invoke(nameof(ResetAttack), TimebetweenAttacks);
            Rigidbody rb = Instantiate(projitile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
        }
    }

    public void ResetAttack()
    { alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    { health -= damage;

        if (health <= 0)
            Invoke(nameof(DestroyEnemy), 0.5f);

    }

    private void DestroyEnemy()
    { Destroy(gameObject);
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

//using UnityEditor.ShaderGraph.Internal;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemyAI : MonoBehaviour
{

    [Header("Core")]
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask WhatisGround, whatisPlayer;


    [Header("Patrol Waypoints")]
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;

    [Header("Attacking")]
    public float TimebetweenAttacks = 5f;
    bool alreadyAttacked;
    public GameObject Bullet;
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

    [Header("Animator")]
    public Animator enemyAnimator;

    [Header("Falling check")]
    public float groundCheckDistance = 0.6f;
    public LayerMask groundLayer;

    [Header("Sitting Behavior")]
    public float sitChancePerPatrol = 0.05f;
    public float sitDurationMin = 3f;
    public float sitDurationMax = 8f;
    public float chairSearchRadius = 8f;
    public bool isSitting = false;




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
        //Its for updating animator parameters every frame
        UpdateAnimatorParameters();

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

    private void UpdateAnimatorParameters()
    {
        if (enemyAnimator == null) return;

        float speed = agent.velocity.magnitude;
        enemyAnimator.SetFloat("Speed", speed);

        // when IsFalling : raycast downward to detect air (if a Large gap, or falling
        bool isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, groundCheckDistance, groundLayer);
        enemyAnimator.SetBool("IsFalling", !isGrounded);

        enemyAnimator.SetBool("isSitting", isSitting);

        enemyAnimator.SetBool("HasGun", playerInSightRange);
    }

    // the function below patrol supports patrol
    private void Patroling()
    {
        if (isSitting) return;
        if (waypoints.Length == 0) return;

        agent.SetDestination(waypoints[currentWaypointIndex].position);

        // Check if enemy reached waypoint
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;

            if (Random.value < sitChancePerPatrol)
            {
                TrySitNearChair();
            }

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
        agent.isStopped = true;
        agent.SetDestination(transform.position);

        // Rotate towards player
        Vector3 lookPos = player.position;
        lookPos.y = transform.position.y;
        transform.LookAt(player);

        // Play get gun stance if needed

        if (enemyAnimator != null)
        {
            enemyAnimator.SetBool("HasGun", true);
        }

        if (!alreadyAttacked)
        {
            // Attack cooldown
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), TimebetweenAttacks);

            // Trigger shooting animation
            if (enemyAnimator != null)

            {
                enemyAnimator.SetTrigger("shoot");
            }
            // 🔹 Raycast for hit detection
            RaycastHit hit;
            if (Physics.Raycast(gunpoint.position, transform.forward, out hit, 100))
            {
                Debug.DrawRay(gunpoint.position, transform.forward * hit.distance, Color.yellow);

                // Muzzle flash
                GameObject a = Instantiate(fire, gunpoint.position, Quaternion.identity);
                Destroy(a, 1);
                Instantiate(Bullet, gunpoint.transform.position, gunpoint.transform.rotation);

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
    {
        alreadyAttacked = false;
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
    {
        Destroy(gameObject);
    }


    private void UpdateHealthBar()
    {
        float percentHealth = this.currentHealth / this.maxHealth;
        this.HealthBar.UpdateHealthBarAmount(percentHealth);
    }

    private void TrySitNearChair()
    {
        Collider[] nearby = Physics.OverlapSphere(transform.position, chairSearchRadius);
        Transform foundChair = null;
        float bestDistance = Mathf.Infinity;

        foreach (Collider c in nearby)
        {
            if (c.CompareTag("Chair"))
            {
                float d = Vector3.Distance(transform.position, c.transform.position);
                if (d < bestDistance)
                {
                    bestDistance = d;
                    foundChair = c.transform;
                }
            }
        }

        if (foundChair != null)
        {
            StartCoroutine(GoToChairAndSit(foundChair));
        }


    }

    private IEnumerator GoToChairAndSit(Transform chair)
    {
        isSitting = false;
        agent.isStopped = false;
        agent.SetDestination(chair.position);

        while (agent.pathPending || agent.remainingDistance > 0.6) ;
        yield return null;

        isSitting = true;
        agent.isStopped = true;

        //Wait random sit duration
        float t = Random.Range(sitDurationMin, sitDurationMax);
         if (enemyAnimator != null)
            enemyAnimator.SetBool("IsSitting", true);
        yield return new WaitForSeconds(t);

        isSitting = false;

        if (enemyAnimator != null)
            enemyAnimator.SetBool("IsSitting", false);
        agent.isStopped = false;
            
            }
}

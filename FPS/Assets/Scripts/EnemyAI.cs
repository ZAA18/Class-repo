//using UnityEditor.ShaderGraph.Internal;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemyAI : MonoBehaviour
{

    [Header("Core")]
    public NavMeshAgent agent;
    public GameObject player;
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

    private bool isDead = false;

    [Header("Backup / communication")]
    public float alertRange = 15f;
    public AudioClip alertSound;
    private AudioSource audioSource;

    public void Awake()
    {
       
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = false;
        this.HealthBar = this.GetComponentInChildren<PlayerHealth>();
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = false;
        if (player == null)

            //player = GameObject.Find("Player")?.transform; 

            //For the sound
           
        

        if (agent == null) 
            agent = GetComponent<NavMeshAgent>();
        if (enemyAnimator == null)
            enemyAnimator = GetComponent<Animator>();

        //For the sound

        audioSource = GetComponent<AudioSource>();
    }

    //Yell to enemies nearby

    public void AlertNearbyEnemies()
    {
        Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, alertRange);

        foreach (Collider col in nearbyEnemies)
        {
            EnemyAI ally = col.GetComponent<EnemyAI>();

            if ( ally != null  && ally != this   && !ally.playerInSightRange)
            {
                ally.OnAlerted(player.transform.position);
            }
        }

        if (audioSource != null && alertSound != null)
        {
            audioSource.PlayOneShot(alertSound);
        }
    }

    public void OnAlerted(Vector3 alertPosition)
    {
        playerInSightRange = true;
        agent.SetDestination(alertPosition);
    }



    // Update is called once per frame
    void Update()

    {
        if (isDead) return;
       
        //Its for updating animator parameters every frame
        UpdateAnimatorParameters();

        playerInSightRange = Physics.CheckSphere(transform.position, sightrange, whatisPlayer);
        playerinAttackRange = Physics.CheckSphere(transform.position, attackrange, whatisPlayer);

        if (!playerInSightRange && !playerinAttackRange)
        { Patroling(); }
        else if (playerInSightRange && !playerinAttackRange)
        {
            AlertNearbyEnemies();
            ChasePlayer();
        }
        else if (playerinAttackRange && playerInSightRange)
        {
            AttackPlayer();
            Debug.Log("Attacking Player");
        }
    
    }


    //Testing if movement works

    private void UpdateAnimatorParameters()
    {
        if (enemyAnimator == null || agent ==null) return;

        float speed = agent.velocity.magnitude;
        enemyAnimator.SetFloat("Speed", speed);

        // when IsFalling : raycast downward to detect air (if a Large gap, or falling
        bool isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, groundCheckDistance, groundLayer);
        enemyAnimator.SetBool("IsFalling", !isGrounded);

        enemyAnimator.SetBool("isSitting", isSitting);

        enemyAnimator.SetBool("HasGun", playerInSightRange);

        //Running when chase (player see but not in attackRange)
        bool isRunning = playerInSightRange && !playerinAttackRange;
        enemyAnimator.SetBool("IsRunning", isRunning);
    }

    // the function below patrol supports patrol
    private void Patroling()
    {
        if (isSitting || waypoints.Length == 0) return;
        

        agent.SetDestination(waypoints[currentWaypointIndex].position);

        // Check if enemy reached waypoint
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;

            if (Random.value < sitChancePerPatrol)
            {
                TrySitNearChair();
                return;
            }

            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }


    }



    private void ChasePlayer()
    {
        isSitting = false;
        agent.isStopped = false;
        if (player != null)
        agent.SetDestination(player.transform.position); }

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
        player = GameObject.FindGameObjectWithTag("Player");
        // Rotate towards player
        //Vector3 lookPos = player.transform.position;
        //lookPos.y = transform.position.y;
        transform.LookAt(player.transform.position);

        // Play get gun stance if needed
        //calling alert function
        AlertNearbyEnemies();

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
            if (Physics.Raycast(gunpoint.position, gunpoint.forward, out hit, 100))
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
        if (isDead)
            return;
        {
            
        }
        this.UpdateHealthBar();
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
            currentHealth = 0;
            Invoke(nameof(DestroyEnemy), 0f);
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        agent.isStopped = true;

        // trigger death animation 
        if (enemyAnimator != null)

        {
            enemyAnimator.SetBool("Die", true);
            enemyAnimator.SetBool("IsFalling", false);
            enemyAnimator.SetFloat("Speed", 0f);


        }

        // disable colliders so physics doesnt interfere (optional)
        Collider col = GetComponent<Collider>();

        if (col != null) col.enabled = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.mass = 2f;
        //Destroy After a delay to allow animation to play
        Destroy(gameObject, 10f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }


    private void UpdateHealthBar()
    { if (HealthBar == null)
            return;

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

   /* private IEnumerator GoToChairAndSit (Transform chair)
    {
        isSitting = false;
        agent.isStopped = false;
        agent.SetDestination(chair.position);

        while (agent.pathPending || agent.remainingDistance > 0.6f)
            yield return null;

        isSitting = true;
        agent.isStopped = true;

        if (enemyAnimator != null) enemyAnimator.SetBool("isSitting", true);
                float t = Random.Range(sitDurationMin, sitDurationMax);
        yield return new WaitForSeconds(t); ;

        isSitting = false;

        if (enemyAnimator != null)
            enemyAnimator.SetBool("IsSitting", false);
        agent.isStopped = false;
    }*/
}

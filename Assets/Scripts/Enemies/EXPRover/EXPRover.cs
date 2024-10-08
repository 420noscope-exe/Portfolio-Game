using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXPRoverAI : MonoBehaviour
{
    [SerializeField]private List<AudioClip> clips = new List<AudioClip>();
    [SerializeField]private List<AudioClip> hit = new List<AudioClip>();

    private AudioSource aSource;
    
    private Animator animator;

    //Navmesh variables
    public UnityEngine.AI.NavMeshAgent agent;

    private GameObject player;

    [SerializeField]private Barrel barrel;

    public LayerMask whatIsGround, whatIsPlayer;

    //moving
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks, timBetweenVoiceLines, timeBetweenPatrol;
    bool alreadyAttacked, alreadyVoiceLined, alreadyPatrolled, alreadyHitPlayer;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    // Start is called before the first frame update

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = gameObject.GetComponentInChildren<Animator>();
        aSource = gameObject.GetComponent<AudioSource>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        barrel = gameObject.GetComponentInChildren<Barrel>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange && !alreadyPatrolled) patrolling();
        if (playerInSightRange && !playerInAttackRange && !alreadyAttacked) chasePlayer();
        if (playerInSightRange && playerInAttackRange) attackPlayer();
    }

    private void FixedUpdate()
    {

    }

    //The three states for the state machine
    private void patrolling()
    {
        if(!walkPointSet) 
            searchWalkPoint();

        if(walkPointSet)
            agent.SetDestination(walkPoint);
            //gameObject.transform.LookAt(walkPoint);
            walk();

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if(distanceToWalkPoint.magnitude <1f)
        {
            walkPointSet = false;
            if(!alreadyPatrolled)
            {
                idle();
                alreadyPatrolled = true;
                Invoke(nameof(resetPatrol), timeBetweenPatrol);
            }
        }    

        if(!alreadyVoiceLined)
            {
                //playVoiceLine();
                alreadyVoiceLined = true;
                Invoke(nameof(resetVoiceline), timBetweenVoiceLines);
            }

    }

    private void chasePlayer()
    {
        UnityEngine.AI.NavMeshPath navMeshPath = new UnityEngine.AI.NavMeshPath();
        agent.CalculatePath(player.transform.position, navMeshPath);

        if(navMeshPath.status == UnityEngine.AI.NavMeshPathStatus.PathComplete)
        {
            agent.SetDestination(player.transform.position);
            Vector3 temp = new Vector3(player.transform.position.x, gameObject.transform.position.y, player.transform.position.z);
            gameObject.transform.LookAt(temp);
            walk();
            if(!alreadyVoiceLined)
              {
                   //playVoiceLine();
                   alreadyVoiceLined = true;
                   Invoke(nameof(resetVoiceline), timBetweenVoiceLines);
               }
        }
        else if(!alreadyPatrolled)
        {
            patrolling();
        }
    }

    private void attackPlayer()
    {
        agent.SetDestination(transform.position);

        if(!alreadyAttacked)
        {
            barrel.Explode();
            resetHitPlayer();
            agent.Stop();
            attack();
            alreadyAttacked = true;
            Invoke(nameof(resetAttack), timeBetweenAttacks);
            Invoke(nameof(resumeAgent), timeBetweenAttacks);
        }    
    }

    //Search walkpoint for navmesh, helper function for patrolling
    private void searchWalkPoint()
    {
        //calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        UnityEngine.AI.NavMeshPath navMeshPath = new UnityEngine.AI.NavMeshPath();
        agent.CalculatePath(walkPoint, navMeshPath);

        if (Physics.Raycast(walkPoint, -transform.up, 5f, whatIsGround) && navMeshPath.status == UnityEngine.AI.NavMeshPathStatus.PathComplete)
        {
            walkPointSet = true;
        }
    }

    //Damages any HealthController that collides with thing GameObject while it is attacking.
    public void OnCollisionEnter(Collision col)
    {
        if(alreadyAttacked && !alreadyHitPlayer && col.gameObject.GetComponent<HealthController>() != null)
        {
            col.gameObject.GetComponent<HealthController>().TakeDamage(20);
            alreadyHitPlayer = true;
        }
    }

    //reset functions for State Machine
    private void resetAttack()
    {
        alreadyAttacked = false;
    }

    private void resetVoiceline()
    {
        alreadyVoiceLined = false;
    }

    private void resetPatrol()
    {
        alreadyPatrolled = false;
    }

    private void resetHitPlayer()
    {
        alreadyHitPlayer = false;
    }

    public void resumeAgent()
    {
        if(gameObject != null && agent.enabled == true)  //This prevents "Resume" error if enemy is killed while attacking
        {
        agent.Resume();
        }
    }
    //Animations
    private void attack()
    {
        animator.Play("Attack");
    }

    private void idle()
    {
        animator.Play("Idle");
    }

    private void walk()
    {
        animator.Play("Walk");
        //playVoiceLine();
    }

    //Idle and walk voicelines
    private void playVoiceLine()
    {
        aSource.clip = clips[Random.Range(0,clips.Count)];
        aSource.Play();
    }




}

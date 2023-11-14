using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class VsauceBossAI : MonoBehaviour
{
    [SerializeField]private List<AudioClip> clips = new List<AudioClip>();
    [SerializeField]private List<AudioClip> hit = new List<AudioClip>();
    [SerializeField]private int action;
    [SerializeField]private AudioClip slagAC;
    [SerializeField]private AudioClip thermiteBallsAC;
    [SerializeField]private AudioClip thermiteBallsShortAC;
    [SerializeField]private AudioClip punchAC;
    [SerializeField]private AudioClip teleportAC;
    [SerializeField]private GameObject explosionEffect;
    [SerializeField]private GameObject rightHand;
    private AudioSource aSource;
    
    private Animator animator;

    //Navmesh variables
    public UnityEngine.AI.NavMeshAgent agent;

    private GameObject player;

    public LayerMask whatIsGround, whatIsPlayer;

    //moving
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

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
    }

    // Update is called once per frame
    void Update()
    {
        if(action == 1)
        {
            idle();
        }
        else if(action == 2)
        {
            walk();
        }
        else if(action == 3)
        {
            punchL();
        }
        else if(action == 4)
        {
            punchR();
        }
        else if(action == 5)
        {
            thermiteBallsShort();
        }
        else if(action == 6)
        {
            slag();
        }
        action = 0;
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) patrolling();
        if (playerInSightRange && !playerInAttackRange) chasePlayer();
        if (playerInSightRange && playerInAttackRange) attackPlayer();
    }

    private void patrolling()
    {
        if(!walkPointSet) 
            searchWalkPoint();

        if(walkPointSet)
            agent.SetDestination(walkPoint);
            gameObject.transform.LookAt(walkPoint);
            walk();

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if(distanceToWalkPoint.magnitude <1f)
            walkPointSet = false;
            stopWalking();
    }

    private void searchWalkPoint()
    {
        //calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 5f, whatIsGround))
        {
            playVoiceLine();
            walkPointSet = true;
        }
    }

    private void chasePlayer()
    {
        agent.SetDestination(player.transform.position);
        Vector3 temp = new Vector3(player.transform.position.x, gameObject.transform.position.y, player.transform.position.z);
        gameObject.transform.LookAt(temp);
        walk();
    }

    private void attackPlayer()
    {
        punchR();
    }

    private void idle()
    {
        animator.Play("Base.Idle");
    }

    private void walk()
    {
        animator.Play("Walk");
        //playVoiceLine();
    }

    private void punchL()
    {
        animator.Play("Base.PunchL");
        aSource.clip = punchAC;
        aSource.Play();
    }

    private void punchR()
    {
        animator.Play("Base.PunchR");
        aSource.clip = punchAC;
        aSource.Play();
    }

    private void thermiteBallsShort()
    {
        animator.Play("Base.ThermiteBalls");
        aSource.PlayOneShot(thermiteBallsShortAC);
        StartCoroutine(thermiteExplosion());
    }

    private void slag()
    {
        animator.Play("Base.Slag");
        aSource.PlayOneShot(slagAC);
    }

    IEnumerator thermiteExplosion()
    {
        float delayInMs = 0.6f;
        float ms = Time.deltaTime;

        while(ms <= delayInMs )
            {
                ms += Time.deltaTime;
                yield return null;
            }
        GameObject temp = Instantiate(explosionEffect, rightHand.transform);
        Destroy(temp, 1.0f);
    }

    IEnumerator stopWalking()
    {
        float delayInMs = 2000f;
        float ms = Time.deltaTime;
        idle();
        while(ms <= delayInMs )
            {
                ms += Time.deltaTime;
                yield return null;
            }
    }

    private void playVoiceLine()
    {
        aSource.PlayOneShot(clips[Random.Range(0,clips.Count)]);
    }
}

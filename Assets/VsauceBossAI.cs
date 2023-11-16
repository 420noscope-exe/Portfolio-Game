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
    [SerializeField]private GameObject slagProj;
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
    public float timeBetweenAttacks, timBetweenVoiceLines, timeBetweenPatrol;
    bool alreadyAttacked, alreadyVoiceLined, alreadyPatrolled;
    public float dashSpeed = 10;

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

        if (!playerInSightRange && !playerInAttackRange && !alreadyPatrolled) patrolling();
        if (playerInSightRange && !playerInAttackRange && !alreadyAttacked) chasePlayer();
        if (playerInSightRange && playerInAttackRange) attackPlayer();
    }

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
                playVoiceLine();
                alreadyVoiceLined = true;
                Invoke(nameof(resetVoiceline), timBetweenVoiceLines);
            }

    }

    private void searchWalkPoint()
    {
        //calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 5f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void chasePlayer()
    {
        agent.SetDestination(player.transform.position);
        Vector3 temp = new Vector3(player.transform.position.x, gameObject.transform.position.y, player.transform.position.z);
        gameObject.transform.LookAt(temp);
        walk();
        if(!alreadyVoiceLined)
            {
                playVoiceLine();
                alreadyVoiceLined = true;
                Invoke(nameof(resetVoiceline), timBetweenVoiceLines);
            }
    }

    private void attackPlayer()
    {
        agent.SetDestination(transform.position);

        if(!alreadyAttacked)
        {
            int randomAttack = Random.Range(1,4);
            switch(randomAttack)
            {
                case 1:
                    combo();
                    break;
                case 2:
                    thermiteBallsShort();
                    break;
                case 3:
                    slag();
                    break;
            }
            alreadyAttacked = true;
            Invoke(nameof(resetAttack), timeBetweenAttacks);
            Invoke(nameof(resumeAgent), timeBetweenAttacks);
        }    
    }

//types of attacks
    private void combo()
    {
        agent.Stop();
            punchR();
            Invoke(nameof(punchL), 1.0f);
            Invoke(nameof(punchR), 2.0f);
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

        StartCoroutine("spraySlag");
    }


//end of types of attacks

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
        StartCoroutine("dash");
    }

    private void punchR()
    {
        animator.Play("Base.PunchR");
        aSource.clip = punchAC;
        aSource.Play();
        StartCoroutine("dash");
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

    IEnumerator spraySlag()
    {
        Vector3 temp = new Vector3(player.transform.position.x, gameObject.transform.position.y, player.transform.position.z);
        transform.LookAt(temp);
        float delay = 1.5f;
        float start = Time.time;
        float fireRate =  1f/(6000f/60f); //value in middle is measured in Rounds per minute
        float nextFire = 0;
        while(Time.time <= start + delay)
            {
                if (Time.time > nextFire)
                {
                    nextFire = Time.time + fireRate;
                    Instantiate(slagProj, rightHand.transform.position, gameObject.transform.rotation);
                    temp = new Vector3(player.transform.position.x, gameObject.transform.position.y, player.transform.position.z);
                    transform.LookAt(temp);
                }
                yield return null;
            }
    }

    private void playVoiceLine()
    {
        aSource.PlayOneShot(clips[Random.Range(0,clips.Count)]);
    }

    IEnumerator dash()
    {
        Vector3 temp = new Vector3(player.transform.position.x, gameObject.transform.position.y, player.transform.position.z);
        transform.LookAt(temp);
        float delay = 1f;
        float start = Time.time;
        while(Time.time <= start + delay)
            {
                gameObject.transform.position += gameObject.transform.forward * dashSpeed * Time.deltaTime;
                yield return null;
            }
    }

    public void resumeAgent()
    {
        agent.Resume();
    }
}

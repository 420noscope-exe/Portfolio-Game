using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VsauceBossHealthController : MonoBehaviour, HealthController
{

    public int health;
    public int maxHealth = 320;
    public VsauceBossAI vsauceAI;
    public UnityEngine.AI.NavMeshAgent agent;
    public AudioSource aSource;
    public AudioClip death, hitHurt;

    private bool deathClipPlayed = false;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        vsauceAI = gameObject.GetComponent<VsauceBossAI>();
        agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        aSource = gameObject.GetComponent<AudioSource>();
    }

    // FixedUpdate is called once every .02 seconds
    void FixedUpdate()
    {
        MaxHealthCheck();
        Kill();
    }

    public bool IsDead() //checks to see if the player should be dead
    {
        if(health <= 0)
        {
            health = 0;
            return true;
        }

        return false;
    }

    public void TakeDamage(int damage) //for taking damage
    {
        health = health - damage;
    }

    public void TakeHeal(int heal) //for healing or increasing health
    {
        health += heal;
    }

    public void Kill() //Kills player is they are supposed to be dead, disables controls, and bring up DeathMenu
    {
        if(IsDead())
        {
            vsauceAI.enabled = false;
            agent.Stop();
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            if(!deathClipPlayed)
            {
                aSource.clip = death;
                aSource.Play();
                deathClipPlayed = true;
            }
            this.enabled = false;
        }
        
    }

    public void MaxHealthCheck()  //checks to see if player is over maxHealth, and will set health=maxhealth if this happens
    {
        if(health > maxHealth)
        {
            health = maxHealth;
        }
    }
}

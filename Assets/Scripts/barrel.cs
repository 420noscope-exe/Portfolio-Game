using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barrel : MonoBehaviour, HealthController
{
    [SerializeField] private AudioClip clip;

    public int health;
    public int maxHealth = 2;

    public GameObject explosionEffect;
    [SerializeField] private int damage = 10;
    public float radius = 5f;
    public float explosionForce = 10000f;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // FixedUpdate is called once every .02 seconds
    void FixedUpdate()
    {
        maxHealthCheck();
        kill();
    }

    public bool isDead() //checks to see if should be dead
    {
        if(health <= 0)
        {
            health = 0;
            return true;
        }

        return false;
    }

    public void takeDamage(int damage) //for taking damage
    {
        health = health - damage;
    }

    public void takeHeal(int heal) //for healing or increasing health
    {
        print("You can't heal a barrel, you idiot!");
    }

    public void kill() 
    {
        if(isDead())
        {
            AudioSource.PlayClipAtPoint(clip, GameObject.Find("Player").transform.position, 1.0f);
            explode();
        }
        
    }

    void explode()
    {
        GameObject spawnedEffect = Instantiate(explosionEffect, gameObject.transform.position, gameObject.transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, radius);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].GetComponent<Rigidbody>())
            {
                colliders[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, gameObject.transform.position, radius);
            }
            if (colliders[i].gameObject.GetComponent<HealthController>() != null)
            {
                colliders[i].gameObject.GetComponent<HealthController>().takeDamage(damage);
                GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>().Play();
            }
        }

        Destroy(spawnedEffect , 1f);
        Destroy(gameObject);
    }

    public void maxHealthCheck()  //checks to see if health is over maxHealth, and will set health=maxhealth if this happens
    {
        if(health > maxHealth)
        {
            health = maxHealth;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{

    public int health;
    public int maxHealth = 100;
    public PlayerController playerController;
    public GameObject DeathMenu;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // FixedUpdate is called once every .02 seconds
    void FixedUpdate()
    {
        maxHealthCheck();
        killPlayer();
    }

    bool isDead() //checks to see if the player should be dead
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
        health += heal;
    }

    void killPlayer() //kills player is they are supposed to be dead, disables controls, and bring up DeathMenu
    {
        if(isDead())
        {
            Cursor.lockState = CursorLockMode.None;
            playerController.enabled = false;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            DeathMenu.SetActive(true);
            //gameObject.GetComponent<HealthController>().enabled = false;
        }
        
    }

    void maxHealthCheck()  //checks to see if player is over maxHealth, and will set health=maxhealth if this happens
    {
        if(health > maxHealth)
        {
            health = maxHealth;
        }
    }
}

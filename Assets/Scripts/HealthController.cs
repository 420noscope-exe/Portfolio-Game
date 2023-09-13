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
        killPlayer();
    }

    bool isDead()
    {
        if(health <= 0)
        {
            health = 0;
            return true;
        }

        return false;
    }

    public void takeDamage(int damage)
    {
        health = health - damage;
    }

    public void takeHeal(int heal)
    {
        health += heal;
    }

    void killPlayer()
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour, HealthController
{

    [SerializeField] private int health;
    [SerializeField] private int maxHealth = 100;
    private PlayerController playerController;
    private Image healthMeter;
    private GameObject deathMenu;
    private AudioSource aSource;
    [SerializeField] private AudioClip hitHurt;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        aSource = gameObject.GetComponent<AudioSource>();
        playerController = gameObject.GetComponent<PlayerController>();
        healthMeter = GameObject.Find("HealthMeter").GetComponent<Image>();
        deathMenu = GameObject.Find("DeathMenu");
        deathMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        SetHealthMeter();
    }
    
    // FixedUpdate is called once every .02 seconds
    void FixedUpdate()
    {
        MaxHealthCheck();
        Kill();
    }

    private void SetHealthMeter()
    {
        float healthPercentage = (float)health/maxHealth;
        healthMeter.fillAmount = healthPercentage;
    }

    public void MaxHealthCheck()  //checks to see if player is over maxHealth, and will set health=maxhealth if this happens
    {
        if(health > maxHealth)
        {
            health = maxHealth;
        }
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
        if(!IsDead())
        {
        health = health - damage;
        aSource.PlayOneShot(hitHurt);
        }
    }

    public void TakeHeal(int heal) //for healing or increasing health
    {
        health += heal;
    }

    public void Kill() //kills player is they are supposed to be dead, disables controls, and bring up DeathMenu
    {
        if(IsDead())
        {
            Cursor.lockState = CursorLockMode.None;
            playerController.enabled = false;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            deathMenu.SetActive(true);
            //gameObject.GetComponent<HealthController>().enabled = false;
        }
        
    }
}

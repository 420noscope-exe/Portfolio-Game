using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shotGun : MonoBehaviour
{
    private AudioSource aSource;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject muzzle;

    private Animator animator;

    float fireRate =  1f/(300f/60f); //value in middle is measured in Rounds per minute
    float nextFire;
    int magazineSize = 2;
    int ammoLoaded;
    float reloadTime = 1;
    float nextReload;

    // Start is called before the first frame update
    void Start()
    {
        aSource = gameObject.GetComponent<AudioSource>();
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire && ammoLoaded > 0 && Time.time > nextReload)
        {
            nextFire = Time.time + fireRate;
            fire();
            ammoLoaded--;
        }
        if (ammoLoaded <= 0 || Input.GetButton("Reload"))
        {
            animator.Play("Reload");
            nextReload = Time.time + reloadTime;
            ammoLoaded = magazineSize;
        }
    }

    public void fire()
    {
        animator.Play("Fire");
        aSource.Play();

        for(int i=0; i<9; i++)
        {
            Instantiate(bullet, muzzle.gameObject.transform.position, gameObject.transform.rotation);
        }
        GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * -100f, ForceMode.Impulse);
    }
}

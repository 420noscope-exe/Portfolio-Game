using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smg : MonoBehaviour, Gun
{
    private AudioSource aSource;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject muzzle;

    private Animator animator;

    float fireRate =  1f/(900f/60f); //value in middle is measured in Rounds per minute
    float nextFire;
    int magazineSize = 30;
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
        if (Input.GetButton("Fire1") && Time.time > nextFire && ammoLoaded > 0 && Time.time > nextReload)
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
        Instantiate(bullet, muzzle.gameObject.transform.position, gameObject.transform.rotation);
    }

    public float getAmmo()
    {
        return (float)ammoLoaded/magazineSize;
    }
}

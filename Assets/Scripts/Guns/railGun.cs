using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class railGun : MonoBehaviour, Gun
{
    private AudioSource aSource;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject muzzle;
    [SerializeField] private AudioClip chargeSound, fireSound, chargingSound;

    private Animator animator;

    float fireRate =  1f/(600f/60f); //value in middle is measured in Rounds per minute
    float nextFire;
    float startCharge;
    bool isCharged, isCharging;
    int magazineSize = 5;
    int ammoLoaded;
    float reloadTime = 1;
    float nextReload;

    // Start is called before the first frame update
    void Start()
    {
        aSource = gameObject.GetComponent<AudioSource>();
        animator = gameObject.GetComponent<Animator>();
        isCharged = false;
        isCharging = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > nextReload)
        {
            startCharge = Time.time;
            aSource.clip = chargingSound;
            aSource.Play();
            isCharging = true;
        }
        if (Input.GetButton("Fire1") && isCharging && Time.time > nextReload && Time.time - startCharge >= 1f && isCharged == false)
        {
            print("RailGun is Charged");
            aSource.clip = chargeSound;
            isCharged = true;
            aSource.Play();
        }
        if (Input.GetButtonUp("Fire1") && Time.time > nextFire && ammoLoaded > 0 && Time.time > nextReload && isCharged)
        {
            nextFire = Time.time + fireRate;
            fire();
            ammoLoaded--;
            isCharging = false;
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
        isCharged = false;
        animator.Play("Fire");
        aSource.clip = fireSound;
        aSource.Play();
        Instantiate(bullet, muzzle.gameObject.transform.position, gameObject.transform.rotation);
    }

    public float getAmmo()
    {
        return (float)ammoLoaded/magazineSize;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocketLauncher : MonoBehaviour, Gun
{
    private AudioSource aSource;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject muzzle;
    private Camera playerCam;
    private PlayerHealthController playerHealthController;

    private Animator animator;

    float fireRate =  1f/(100f/60f); //value in middle is measured in Rounds per minute
    float nextFire;
    int magazineSize = 1;
    int ammoLoaded;
    float reloadTime = 2;
    float nextReload;
    private int range = 600;

    // Start is called before the first frame update
    void Start()
    {
        aSource = gameObject.GetComponent<AudioSource>();
        animator = gameObject.GetComponent<Animator>();
        playerCam = GameObject.FindWithTag("Player").GetComponentInChildren<Camera>();
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
        Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
        Physics.Raycast(ray, out RaycastHit hit, range);
        animator.Play("Fire");
        aSource.Play();
        if(hit.point == Vector3.zero)
        {
            hit.point = playerCam.transform.position + playerCam.transform.forward * range;
        }
        muzzle.transform.LookAt(hit.point);
        print(muzzle.transform.rotation);
        Instantiate(bullet, muzzle.gameObject.transform.position, Quaternion.LookRotation(muzzle.transform.forward));
    }

    public float getAmmo()
    {
        return (float)ammoLoaded/magazineSize;
    }
}

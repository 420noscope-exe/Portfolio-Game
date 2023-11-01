using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class railGun : MonoBehaviour, Gun
{
    private AudioSource aSource;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject muzzle;
    // Start is called before the first frame update
    void Start()
    {
        aSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        fire();
    }

    public void fire()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            aSource.Play();
            Instantiate(bullet, muzzle.gameObject.transform.position, gameObject.transform.rotation);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugGun : MonoBehaviour, Gun
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject muzzle;
    // Start is called before the first frame update
    void Start()
    {
        
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
            print("Debug gun has been fired.");
            Instantiate(bullet, muzzle.gameObject.transform.position, gameObject.transform.rotation);
        }
    }
}

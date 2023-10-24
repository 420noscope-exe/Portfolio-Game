using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugGun : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fire();
    }

    void fire()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            print("Debug gun has been fired.");
            Instantiate(bullet, gameObject.transform.position, gameObject.transform.rotation);
        }
    }
}

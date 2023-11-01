using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class railGunPickup : MonoBehaviour
{
    [SerializeField]private GameObject gunPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            print("Player picked up railgun.");
            col.gameObject.GetComponent<WeaponController>().pickupGun(gunPrefab);
            Destroy(gameObject);
        }
    }
}

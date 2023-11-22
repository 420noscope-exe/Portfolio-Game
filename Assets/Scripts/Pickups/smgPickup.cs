using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smgPickup : MonoBehaviour
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
            print("Player picked up smg.");
            col.gameObject.GetComponent<WeaponController>().pickupGun(gunPrefab);
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour, Pickup
{
    HealthController healthController;
    int healAmount = 20;

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
        if(col.gameObject.GetComponent<HealthController>() != null)
        {
            print("Health has been picked up");
            healthController = col.gameObject.GetComponent<HealthController>();
            healthController.takeHeal(healAmount);
            Destroy(gameObject);
        }
    }
}

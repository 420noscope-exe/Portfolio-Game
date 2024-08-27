using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverHammer : MonoBehaviour
{
    public bool canAttack;
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
        if(canAttack && col.gameObject.GetComponent<HealthController>() != null)
        {
            col.gameObject.GetComponent<HealthController>().TakeDamage(20);
        }
    }
}

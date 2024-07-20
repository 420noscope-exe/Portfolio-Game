using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class railGunBullet : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 velocity;
    private HealthController healthController;
    [SerializeField] private int damage = 10;
    [SerializeField] private float speed = 100;
    [SerializeField] private bool canRicochet = true;
    [SerializeField] private int ricochetCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        velocity = gameObject.transform.forward * speed;
        rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = velocity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = rb.velocity.normalized * speed;
        print("actual: " + velocity + "desired: " + (velocity.normalized * speed));
    }

    private void OnCollisionEnter(Collision col)
    {
        
        if(col.gameObject.GetComponent<HealthController>() != null)
        {
            healthController = col.gameObject.GetComponent<HealthController>();
            healthController.TakeDamage(damage);
            GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>().Play();
        }
        if(!canRicochet)
        {
            Destroy(gameObject);
        }
        if(canRicochet)
        {
            ricochetCount++;
            if(ricochetCount >= 3)
            {
                canRicochet = false;
            }
            Destroy(gameObject,2f);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.attachedRigidbody != null)
        print("RailGun Bullet pierced " + col.gameObject.name);
        if(col.attachedRigidbody != null && col.attachedRigidbody.gameObject.GetComponent<HealthController>() != null)
        {
            healthController = col.attachedRigidbody.gameObject.GetComponent<HealthController>();
            healthController.TakeDamage(damage);
            GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>().Play();
        }
        else if(col.gameObject.GetComponent<HealthController>() != null)
        {
            healthController = col.gameObject.GetComponent<HealthController>();
            healthController.TakeDamage(damage);
            GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>().Play();
        }
    }
}

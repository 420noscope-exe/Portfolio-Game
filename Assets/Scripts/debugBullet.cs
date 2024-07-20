using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugBullet : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 velocity;
    private HealthController healthController;
    [SerializeField] private int damage;
    [SerializeField] private float speed;
    [SerializeField] private GameObject marker;
    [SerializeField] private bool canRicochet;
    // Start is called before the first frame update
    void Start()
    {
        velocity = gameObject.transform.forward * speed;
        rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = velocity;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision col)
    {
        print("Debug Bullet Collided with " + col.gameObject.name + " at " + gameObject.transform.position);
        Instantiate(marker, gameObject.transform.position, gameObject.transform.rotation);
        if(col.gameObject.GetComponent<HealthController>() != null)
        {
            healthController = col.gameObject.GetComponent<HealthController>();
            healthController.TakeDamage(damage);
        }
        if(!canRicochet)
        {
            Destroy(gameObject);
        }
    }
}

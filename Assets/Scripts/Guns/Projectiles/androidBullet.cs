using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class androidBullet : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 velocity;
    private HealthController healthController;
    [SerializeField] private int damage = 10;
    [SerializeField] private float speed = 50;
    [SerializeField] private bool canRicochet = false;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smgBullet : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 velocity;
    private HealthController healthController;
    [SerializeField] private int damage = 5;
    [SerializeField] private float speed = 50;
    [SerializeField] private bool canRicochet = false;
    [SerializeField] private float spread = 0.15f;
    // Start is called before the first frame update
    void Start()
    {
        velocity = (gameObject.transform.forward + randomizeSpread()) * speed;
        rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = velocity;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision col)
    {
        //print("Debug Bullet Collided with " + col.gameObject.name + " at " + gameObject.transform.position);
        if(col.gameObject.GetComponent<HealthController>() != null)
        {
            healthController = col.gameObject.GetComponent<HealthController>();
            healthController.takeDamage(damage);
            GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>().Play();
        }
        if(!canRicochet)
        {
            Destroy(gameObject);
        }
    }

    public Vector3 randomizeSpread()
    {
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        float z = Random.Range(-spread, spread);

        return new Vector3(x,y,z);

    }
}

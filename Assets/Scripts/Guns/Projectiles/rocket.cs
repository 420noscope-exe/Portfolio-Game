using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocket : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 velocity;
    private HealthController healthController;
    [SerializeField] private int damage = 10;
    [SerializeField] private float speed = 50;

    [SerializeField] private AudioClip clip;
    [SerializeField] private GameObject explosionEffect;
    public float radius = 5f;
    public float explosionForce = 10000f;

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
            healthController.takeDamage(damage);
        }
        explode();
    }

    void explode()
    {
        AudioSource.PlayClipAtPoint(clip, GameObject.Find("Player").transform.position, 1.0f);
        GameObject spawnedEffect = Instantiate(explosionEffect, gameObject.transform.position, gameObject.transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, radius);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].GetComponent<Rigidbody>())
            {
                colliders[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, gameObject.transform.position, radius);
            }
        }

        Destroy(spawnedEffect , 1f);
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class DropLoot : MonoBehaviour
{
    [SerializeField]private int dropChance;
    [SerializeField]private GameObject loot;
    private HealthController healthController;
    private bool droppedLoot;

    // Start is called before the first frame update
    void Start()
    {
        healthController = gameObject.GetComponent<HealthController>();
        droppedLoot = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(healthController.IsDead() && droppedLoot == false)
        {
            int rtd = Random.Range(1,101);
            print(rtd);
            if(rtd <= dropChance && loot != null)
            {
                Instantiate(loot, gameObject.transform);
            }
            droppedLoot = true;
        }
    }
}

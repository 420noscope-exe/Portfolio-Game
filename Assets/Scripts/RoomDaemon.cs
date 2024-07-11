using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDaemon : MonoBehaviour
{
    [SerializeField]private List<GameObject> enemies = new List<GameObject>();
    private GameObject PGS;
    private bool roomIsClear;
    // Start is called before the first frame update
    void Start()
    {
        roomIsClear = false;
        GameObject[] findEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in findEnemies)
        {
            enemies.Add(enemy);
        }
        PGS = GameObject.Find("proceduralGenerationSystem");
    }

    // Update is called once per frame
    void Update()
    {
        int deadEnemyCount = 0;

        foreach(GameObject enemy in enemies)
        {
            if (enemy == null || enemy.GetComponent<HealthController>().isDead())
            deadEnemyCount++;
        }

        if (deadEnemyCount == enemies.Count && !roomIsClear)
        {
            PGS.GetComponent<proceduralGenerationSystem>().roomIsClear = true;
            roomIsClear = true;
        }
    }
}

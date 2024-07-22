using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDaemon : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();
    private GameObject PGS;
    [SerializeField] private GameObject door;
    [SerializeField] private float cleanupDistance;
    private DoorController doorController;
    private bool roomIsClear;
    private GameObject player;
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
        doorController = door.GetComponentInChildren<DoorController>();
    }

    // Update is called once per frame
    void Update()
    {
        int deadEnemyCount = 0;

        foreach(GameObject enemy in enemies)
        {
            if (enemy == null || enemy.GetComponent<HealthController>().IsDead())
            deadEnemyCount++;
        }

        if (deadEnemyCount == enemies.Count && !roomIsClear)
        {
            PGS.GetComponent<proceduralGenerationSystem>().roomIsClear = true;
            roomIsClear = true;
            doorController.isUnlocked = true;
        }

        RoomCleanup();
    }

    private void RoomCleanup()
    {
        if(roomIsClear)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            GameObject exit = gameObject.transform.Find("exit").gameObject;
            Vector3 currentDistance = exit.transform.position - player.transform.position;

            if(currentDistance.sqrMagnitude >= Mathf.Pow(cleanupDistance, 2))
            {
                Destroy(gameObject);
            }

        }
    }
}

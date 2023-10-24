using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class proceduralGenerationSystem : MonoBehaviour
{
    [SerializeField] private GameObject spawnRoom;
    [SerializeField] private GameObject hallway;
    //[SerializeField] private static int roomArraySize;
    [SerializeField] private GameObject[] roomArray; //= new GameObject[roomArraySize];
    private Vector3 entrance;
    private Vector3 exit;
    private Vector3 instancePos = new Vector3(0,0,0);
    [SerializeField] private bool roomIsClear;
    private GameObject currentRoom;
    private GameObject lastRoom;

    // Start is called before the first frame update
    void Start()
    {
        roomIsClear = false;
        currentRoom = spawnRoom;
        //generateFirstRoom();
        //findRoomExtents(roomArray[0]);
    }

    // Update is called once per frame
    void Update()
    {
        generateNextRoom();
    }

    private void generateHallway(GameObject room)
    {
        findExit(room);
        instancePos = new Vector3(exit.x,0,(exit.z - 3.15f));
        GameObject hallwayInstance = Instantiate(hallway, instancePos, hallway.transform.rotation);
        print("Instantiated hallway at " + instancePos);
    }

    /*private void generateFirstRoom()
    {
        lastRoom = spawnRoom;
        generateHallway(currentRoom);
        GameObject roomInstance = Instantiate(roomArray[0],)
    }*/

    private void generateNextRoom()
    {
        if(roomIsClear)
        {
            lastRoom = currentRoom;
            currentRoom = getRandomRoom();
            generateHallway(lastRoom);
            instancePos = new Vector3(exit.x,0,(exit.z - 6.0f - findRoomExtents(currentRoom).z));
            print("calculated position");
            GameObject roomInstance = Instantiate(currentRoom, instancePos, currentRoom.transform.rotation);
            print("instantiated room" + instancePos);
            roomIsClear = false;
            currentRoom = roomInstance;

        }
    }

    private Vector3 findRoomExtents(GameObject room)
    {
        Vector3 roomExtents;
        // First find a center for your bounds.
        Vector3 center = Vector3.zero;
        
        foreach (Transform child in room.transform)
        {
            if(child.gameObject.GetComponentInChildren<Renderer>())
            {
                center += child.gameObject.GetComponentInChildren<Renderer>().bounds.center;
            }   
        }
        center /= room.transform.childCount; //center is average center of children

        //Now you have a center, calculate the bounds by creating a zero sized 'Bounds', 
        Bounds bounds = new Bounds(center,Vector3.zero); 

        foreach (Transform child in room.transform)
        {
            if(child.gameObject.GetComponentInChildren<Renderer>())
            {
                bounds.Encapsulate(child.gameObject.GetComponentInChildren<Renderer>().bounds);   
            }
        }
        roomExtents = bounds.extents;
        //print(roomExtents);
        return roomExtents;
    }

    private void findEntrance(GameObject room)
    {
        entrance = room.transform.Find("entrance").position;
    }

    private void findExit(GameObject room)
    {
        exit = room.transform.Find("exit").position;
        print("exit found at " + exit);
    }

    private GameObject getRandomRoom()
    {
        //randomly picks room prefab GameObject from array
        int roomNum = Random.Range(0, roomArray.Length);
        return roomArray[roomNum];
    }
}

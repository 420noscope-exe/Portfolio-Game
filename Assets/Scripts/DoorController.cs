using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Animator door = null;
    public bool isUnlocked;

    public void Start()
    {
        //isUnlocked = false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player") && isUnlocked)
        {
            door.Play("doorOpen", 0, 0f);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if(collider.CompareTag("Player") && isUnlocked)
        {
            door.Play("doorClose", 0, 0f);
        }
    }


}

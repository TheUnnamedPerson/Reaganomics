using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public int rotation;
    public int priority;
    void OnTriggerStay (Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.GetComponent<Character>().partyLeader == true)
            {
                other.GetComponent<PlayerMovement3D>().addRotationToQueue(rotation, priority);
            }
        }
    }
}

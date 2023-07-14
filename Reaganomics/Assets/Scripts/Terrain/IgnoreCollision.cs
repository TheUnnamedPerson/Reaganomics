using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour
{
    public string _tag = "";
    void Start ()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag(tag))
        {
            Physics.IgnoreCollision(go.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == tag)
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRespawn : MonoBehaviour
{
    private Quaternion InitialRotation;
    private Vector3 InitialPosition;
    [SerializeField]
    private bool locked = false;
    public bool Locked
    {
        get
        {
            return locked;
        }
        set
        {
            rb.detectCollisions = !value;
            locked = value;
        }
    }
    private float countDown = 0;
    private Rigidbody rb;
    private float Timer = 15;
    void Start()
    {
        InitialRotation = transform.rotation;
        InitialPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        rb.detectCollisions = !locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (locked) return;
        if (transform.rotation != InitialRotation || InitialPosition != transform.position)
        {
            countDown += Time.deltaTime;
            if (countDown > Timer)
            {
                countDown = 0;
                transform.rotation = InitialRotation;
                transform.position = InitialPosition;
            }
        }
        else countDown = 0;
    }
}

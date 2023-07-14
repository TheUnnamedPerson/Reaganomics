using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerMovement2D : PlayerMovement
{
    public SpriteRenderer sr;
    public Rigidbody2D rb;

    void Start ()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        sr = gameObject.GetComponentInChildren<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        
        steps = new Vector2[stepMultiplier * 5];

        for (int i = 1; i < steps.Length; i++)
        {
            steps[i].x = i * partyDistance / 4f + transform.position.x;
            steps[i].y = transform.position.y;
        }

        int sm = steps.Length / 5;
        for (int i = 0; i < party.Length; i++)
        {
            party[i].transform.position = new Vector3(steps[(i + 1) * sm].x, steps[(i + 1) * sm].y, steps[(i + 1) * sm].y / 2);
        }
    }


    void FixedUpdate ()
    {
        float movementX = (Input.GetKey(KeyCode.D) ? 1 : 0) + (Input.GetKey(KeyCode.A) ? -1 : 0);
        float movementY = (Input.GetKey(KeyCode.W) ? 1 : 0) + (Input.GetKey(KeyCode.S) ? -1 : 0);

        isMoving = (Math.Abs(movementX) + Math.Abs(movementY) != 0);

        isRunning = Input.GetKey(KeyCode.LeftShift);
        if (invertRun) isRunning = !isRunning;

        if (movementX > 0) direction = true; else if (movementX < 0) direction = false;

        Vector2 pos = new Vector2(movementX, movementY) * sensitivity * baseSens;

        pos = Vector2.ClampMagnitude(pos, 1) * (isRunning ? runMultiplier : 1);

        Vector3 newPos = transform.position + new Vector3(pos.x, pos.y, 0);

        if(isMoving == false)
        {
            newPos.x = (float)Math.Round(newPos.x / stepSize) * stepSize;
            newPos.y = (float)Math.Round(newPos.y / stepSize) * stepSize;
        }

        //transform.position = newPos;
        rb.velocity = (newPos - transform.position) / Time.deltaTime;

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y / 2);

        Vector2 p = pos;

        int sm = steps.Length / 5;

        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), steps[0]) > partyDistance / sm)
        {
            for (int i = steps.Length - 1; i > 0; i--)
            {
                steps[i] = (steps[i - 1]);
            }
            steps[0] = transform.position;
        }
        
        for (int i = 0; i < party.Length; i++)
        {
            Vector3 initPos = party[i].transform.position;
            Vector2 np = pos.magnitude * (new Vector3(steps[(i + 0) * sm + 1].x, steps[(i + 0) * sm + 1].y, 0) - party[i].transform.position);
            //if (np.magnitude < stepSize) np = Vector2.ClampMagnitude(np.normalized, stepSize);
            party[i].transform.position += new Vector3(np.x, np.y, 0);
            party[i].transform.position = new Vector3 (party[i].transform.position.x, party[i].transform.position.y, party[i].transform.position.y / 2);
            party[i].transform.GetComponentInChildren<Animator>().SetBool("isMoving", party[i].transform.position != initPos);
            if (party[i].transform.position != initPos) party[i].GetComponentInChildren<SpriteRenderer>().flipX = (party[i].transform.position.x > initPos.x);
        }

    }

    void Update()
    {
        anim.SetBool("isMoving", isMoving);
        sr.flipX = direction;
    }
}

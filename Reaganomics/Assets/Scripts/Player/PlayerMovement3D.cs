using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PlayerMovement3D : PlayerMovement
{
    public SpriteRenderer sr;
    public Rigidbody rb;
    public Cardboarder cb;
    public bool directionY;
    //public bool changedDirY;
    [Range(0, 2)]
    public int rotation;
    public KeyBinds keyBinds;
    public int priority;

    public int prevRot;
    public int controlDirection;

    public float rotationOffset = 0f;

    public float rotationPitch = 0f;
    public bool isSitting = false;

    public List<(int, int)> rotationQueue = new List<(int, int)>();

    void onEnable ()
    {
        keyBinds.Enable();
    }

    void onDisable ()
    {
        keyBinds.Disable();
    }

    void Start ()
    {
        keyBinds = new KeyBinds();
        keyBinds.Enable();
        anim = gameObject.GetComponentInChildren<Animator>();
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        cb = sr.GetComponent<Cardboarder>();
        rb = gameObject.GetComponent<Rigidbody>();
        //rb.freezeRotation = true;
        
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
        bool curDirY = directionY;
        float movementX = keyBinds.PlayerMovement.Movement.ReadValue<Vector2>().x;
        float movementY = keyBinds.PlayerMovement.Movement.ReadValue<Vector2>().y;

        if (inPrompt)
        {
            movementX = 0; movementY = 0;
        }

        //print ("movementX: " + movementX.ToString() + " ; movementY: " + movementY.ToString());
        //float movementX = (Input.GetKey(KeyCode.D) ? 1 : 0) + (Input.GetKey(KeyCode.A) ? -1 : 0);
        //float movementY = (Input.GetKey(KeyCode.W) ? 1 : 0) + (Input.GetKey(KeyCode.S) ? -1 : 0);

        resolveRotation();

        switch (controlDirection)
        {
            case 1:
                float _m = movementY;
                movementY = movementX;
                movementX = _m;
                movementY *= -1;
                break;
            case 2:
                goto case 1;
        }

        switch (rotation)
        {
            case 0:
                if (movementX > 0) direction = true; else if (movementX < 0) direction = false;
                if (movementY > 0) directionY = true; else if (movementY < 0) directionY = false;
                break;
            case 1:
                float _dY = movementX + movementY;
                float _dX = movementX + movementY * -1;
                if (_dX > 0) direction = true; else if (_dX < 0) direction = false;
                if (_dY > 0) directionY = true; else if (_dY < 0) directionY = false;
                break;
            case 2:
                if (movementY > 0) direction = false; else if (movementY < 0) direction = true;
                if (movementX > 0) directionY = true; else if (movementX < 0) directionY = false;
                break;
        }

        isMoving = (Math.Abs(movementX) + Math.Abs(movementY) != 0);

        if (!isMoving)
        {
            priority = 0;
            controlDirection = rotation;
        }

        isRunning = Input.GetKey(KeyCode.LeftShift);
        if (invertRun) isRunning = !isRunning;

        

        Vector2 pos = new Vector2(movementX, movementY) * sensitivity * baseSens;

        pos = Vector2.ClampMagnitude(pos, 1) * (isRunning ? runMultiplier : 1);

        Vector3 newPos = transform.position + (Quaternion.Euler(0, rotationOffset, 0) * new Vector3(pos.x, 0, pos.y));

        if(isMoving == false)
        {
            newPos.x = (float)Math.Round(newPos.x / stepSize) * stepSize;
            newPos.z = (float)Math.Round(newPos.z / stepSize) * stepSize;
        }

        //transform.position = newPos;
        //rb.velocity = (newPos - transform.position) / Time.deltaTime;

        Vector3 sdcmodmk = ((newPos - transform.position) / Time.deltaTime);

        //rb.AddForce(((newPos - transform.position) / Time.deltaTime), ForceMode.VelocityChange);

        rb.velocity = new Vector3(sdcmodmk.x, rb.velocity.y, sdcmodmk.z);

        //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        //Vector2 p = pos;

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
            if (party[i].transform.position != initPos)
            {
                Transform partySprite = party[i].GetComponentInChildren<SpriteRenderer>().transform;
                partySprite.localScale = new Vector3(((party[i].transform.position.x > initPos.x) ? -1 : 1) * partySprite.localScale.x, partySprite.localScale.y, partySprite.localScale.z);
            }
        }
        //changedDirY = (curDirY != directionY);
    }

    void Update()
    {
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isSitting", isSitting);
        //sr.flipX = direction;        
    }

    void LateUpdate ()
    {
        //rb.freezeRotation = false;

        bool _direction = direction;
        bool _directionY = directionY;

        float r = 0;

        switch (rotation)
        {
            case 0:
                r = 0;
                break;
            case 1:
                r = 45;
                break;
            case 2:
                r = 90;
                break;            
        }

        //if (rotation == 1 | rotation == 2)
        //{
        //    _direction = directionY;
        //    _directionY = direction;
        //}

        //transform.rotation = Quaternion.Euler(Vector3.Lerp(new Vector3(0,transform.rotation.y + ((_directionY) ? 180 : 0), 0), new Vector3(0,((_directionY) ? 180 : 0) + r,0), .05f));

        transform.rotation = Quaternion.Euler(new Vector3(0,((_directionY) ? 180 : 0) + r + rotationOffset,0));
        
        
        transform.rotation = Quaternion.Euler(new Vector3((float)Math.Round(transform.rotation.eulerAngles.x / 45f) * 45f, (float)Math.Round(transform.rotation.eulerAngles.y / 45f) * 45f, (float)Math.Round(transform.rotation.eulerAngles.z / 45f) * 45f));

        //rb.freezeRotation = true;
        sr.transform.localScale = new Vector3(((_direction) ? -1 : 1) * ((_directionY) ? -1 : 1) * sr.transform.localScale.x, sr.transform.localScale.y, sr.transform.localScale.z);
        cb.CardBoardify();
    }

    public void addRotationToQueue (int _rotation, int _priority)
    {
        rotationQueue.Add((_rotation, _priority));
    }

    public void setRotationFromSlider (float _r)
    {
        rotation = (int)_r;
        priority += 2;
    }

    public void addRotationToQueue ((int, int) _tuple)
    {
        rotationQueue.Add(_tuple);
    }

    public void resolveRotation ()
    {
        foreach ((int, int) _tuple in rotationQueue)
        {
            if (_tuple.Item2 > priority)
            {
                rotation = _tuple.Item1;
                priority = _tuple.Item2;
            }
        }
        rotationQueue = new List<(int, int)>();
    }
}

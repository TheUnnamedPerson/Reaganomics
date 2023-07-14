using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerMovement : MonoBehaviour
{
    public bool isMoving;
    public bool isRunning;
    public float runMultiplier = 2;
    public float sensitivity = 0.5f;
    public float stepSize = 0.01f;
    public float baseSens = 0.15f;
    public bool invertRun = true;
    public Animator anim;
    public bool direction; //false is left true is right
    public Vector2[] steps;
    public Transform[] party;
    public float partyDistance = 1;
    public int stepMultiplier = 4;
    public bool inPrompt = false;

}

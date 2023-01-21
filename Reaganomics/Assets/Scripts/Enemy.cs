using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameManager gameManager;
    public Character character;
    public EnemyMovement enemyMovement;
    public Transform[] enemies;
    public bool battle = true;
    public Rigidbody2D rb;

    
    void Start()
    {
        character = gameObject.GetComponent<Character>();
        if (!battle) enemyMovement = gameObject.GetComponent<EnemyMovement>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!battle) enemyMovement.inBattle = character.inBattle;
    }

    
}

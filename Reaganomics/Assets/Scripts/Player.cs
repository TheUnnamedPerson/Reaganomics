using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
    public GameManager gameManager;
    public Character character;
    public PlayerMovement playerMovement;
    public Camera transitionCamera;
    public Camera mainCam;
    public Texture2D texture2D;
    public bool partyLeader = false;
    public bool inPrompt = false;

    void Start()
    {
        character = gameObject.GetComponent<Character>();
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        mainCam = gameManager.MainCam;
        float theAngle = Mathf.Round(transform.rotation.eulerAngles.y / 45f) * 45f % 180f;
        mainCam.transform.position = transform.position + (Quaternion.Euler(0,theAngle, 0) * new Vector3(0,6.5f,-9.3f));
        mainCam.transform.rotation = Quaternion.Euler(31.54f, theAngle, 0);
    }

    
    void Update()
    {
        playerMovement.enabled = !(character.inBattle || inPrompt);
    }

    void FixedUpdate ()
    {
        if (!gameManager.battle)
        {
            float theAngle = Mathf.Round(transform.rotation.eulerAngles.y / 45f) * 45f % 180f;
            //mainCam.transform.position = transform.position + (Quaternion.Euler(0,theAngle, 0) * new Vector3(0,6.5f,-9.3f));
            //mainCam.transform.rotation = Quaternion.Euler(31.54f, theAngle, 0);
            //print (transform.position + (Quaternion.Euler(0,theAngle, 0) * new Vector3(0,6.5f,-9.3f)));
            //print (Vector3.Lerp(transform.position, transform.position + (Quaternion.Euler(0,theAngle, 0) * new Vector3(0,6.5f,-9.3f)), .05f));
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, transform.position + (Quaternion.Euler(0,theAngle, 0) * new Vector3(0,6.5f,-9.3f)), .05f);
            mainCam.transform.rotation = Quaternion.Lerp(mainCam.transform.rotation, Quaternion.Euler(31.54f, theAngle, 0), .05f);
        }
        else mainCam.transform.position = Vector3.zero + Vector3.back * 25;
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if (gameManager.battle == false)
        {
            playerMovement.enabled = false;
            GetComponentInChildren<Animator>().SetBool("isMoving", false);
            
            if (other.gameObject.tag == "Enemy")
            {
                StartCoroutine(gameManager.StartBattle(other.GetComponent<Enemy>()));
            }
        }
        
    }

}

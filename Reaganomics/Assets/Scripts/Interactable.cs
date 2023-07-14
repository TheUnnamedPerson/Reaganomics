using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class Interactable : MonoBehaviour
{
    public UnityEvent onRightClick;
    public UnityEvent onLeftClick;
    public bool playerInTrigger = false;
    public bool leftClickSet;
    public bool rightClickSet;
    public GameObject player;
    public GameObject inputIcon;
    public Sprite mouseLeftClick;
    public Sprite mouseRightClick;
    public Sprite mouseBothClick;
    public Sprite mouseNoClick;
    public Player _play;


    void Start ()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");
        
        for (int i = 0; i < objs.Length; i++)
        {
            
            if (objs[i].GetComponent<Player>() != null && objs[i].GetComponent<Player>().partyLeader)
            {
                player = objs[i];
            }
        }

        inputIcon = transform.GetChild(0).gameObject;

        _play = player.GetComponent<Player>();
    }

    void Awake ()
    {
        inputIcon = transform.GetChild(0).gameObject;
    }
    
    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject == player)
        {
            playerInTrigger = true;
            inputIcon.SetActive(true);
        }
    }

    void OnTriggerExit (Collider other)
    {
        if (other.gameObject == player)
        {
            playerInTrigger = false;
            inputIcon.SetActive(false);
        }
    }

    void Update ()
    {
        try { onLeftClick.GetPersistentMethodName(0); leftClickSet = true; }
        catch (System.ArgumentOutOfRangeException) { leftClickSet = false; }

        try { onRightClick.GetPersistentMethodName(0); rightClickSet = true; }
        catch (System.ArgumentOutOfRangeException) { rightClickSet = false; }

        if (leftClickSet && rightClickSet) { inputIcon.GetComponent<SpriteRenderer>().sprite = mouseBothClick; }
        else if (leftClickSet) { inputIcon.GetComponent<SpriteRenderer>().sprite = mouseLeftClick; }
        else if (rightClickSet) { inputIcon.GetComponent<SpriteRenderer>().sprite = mouseRightClick; }
        else { inputIcon.GetComponent<SpriteRenderer>().sprite = mouseNoClick; }

        if(Input.GetMouseButtonDown(0) && playerInTrigger && leftClickSet && !_play.inPrompt)
        {
            onLeftClick.Invoke();
            inputIcon.SetActive(false);
        }
        if(Input.GetMouseButtonDown(1) && playerInTrigger && rightClickSet && !_play.inPrompt)
        {
            onRightClick.Invoke();
            inputIcon.SetActive(false);
        }
    }
}

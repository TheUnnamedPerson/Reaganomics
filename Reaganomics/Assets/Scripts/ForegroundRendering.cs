using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForegroundRendering : MonoBehaviour
{
    public SpriteRenderer[] rends;
    public float[] opacity = {0.5f};
    void Start()
    {
        if (rends.Length == 0) { rends = new SpriteRenderer[1]; rends[0] = transform.GetComponentInChildren<SpriteRenderer>(); }
    }

    
    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.GetComponent<Character>().partyLeader == true) for (int i = 0; i < rends.Length; i++) rends[i].color = new Color(1f,1f,1f,opacity[i]);
        }
    }
    void OnTriggerExit2D (Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.GetComponent<Character>().partyLeader == true) for (int i = 0; i < rends.Length; i++) rends[i].color = new Color(1f,1f,1f,1f);
        }
    }
}

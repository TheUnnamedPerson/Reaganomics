using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public enum PathingType { Patrol, Guard, Wandering, Seeking }
    public PathingType pathingType;

    public NavMeshAgent agent;
    public GameObject player;
    public Transform[] children;
    public bool inBattle;
    public Rigidbody2D rb;

    public Vector3[] cZs;

    void onDisable()
    {
        agent.Stop();
        agent.enabled = false;
    }

    void onEnable()
    {
        agent.enabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
		agent.updateUpAxis = false;
        GameObject[] PMs = GameObject.FindGameObjectsWithTag("Player");
        children = transform.GetComponentsInChildren<Transform>();
        foreach (GameObject go in PMs) if (go.GetComponent<Player>() != null) player = go;
        cZs = new Vector3[children.Length];
        for (int i = 0; i < cZs.Length; i++)
        {
            cZs[i] = children[i].localPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!inBattle)
        {
            agent.enabled = true;
            agent.SetDestination(new Vector3(player.transform.position.x, player.transform.position.y, 50));
        }
        else
        {
            agent.isStopped = true;
            agent.enabled = false;
        }
        for (int i = 0; i < cZs.Length; i++)
        {
            Transform child = children[i];
            if (!inBattle) child.position = new Vector3(child.position.x, child.position.y, transform.position.y / 2 + cZs[i].z);
            else child.localPosition = cZs[i];
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform player;
    public MeshRenderer lastTerrain;
    public Material lastMat;
    public MeshRenderer lastRoof;
    public Material lastRoofMat;
    public float defaultAlpha = .75f;
    public Shader t_Shader;
    public LayerMask terrain;
    public LayerMask roofs;
    void Start()
    {
        player = GameObject.FindObjectOfType<Player>().transform;
    }

    
    void Update()
    {
        CameraTransparency();
    }

    IEnumerator ClearConsole()
    {
        // wait until console visible
        while(!Debug.developerConsoleVisible)
        {
            yield return null;
        }
        yield return null; // t$$anonymous$$s is required to wait for an additional frame, without t$$anonymous$$s clearing doesn't work (at least for me)
        Debug.ClearDeveloperConsole();
    }

    void CameraTransparency (bool ignoreRoof = false)
    {
        RaycastHit hit; //(transform.position - player.position).normalized
        Vector3 pRot = (player.position + Vector3.up * 0.76f - transform.position).normalized * -1;
        bool b = Physics.BoxCast(player.position + Vector3.up * 0.76f - player.transform.forward * (-.125f / 1f), new Vector3(player.lossyScale.x / 2f, player.lossyScale.y / 2f, player.lossyScale.z / 14f) , pRot, out hit, transform.rotation, Vector3.Distance(transform.position, player.position + Vector3.up * 0.76f - player.transform.forward * (-.125f / 1f)), terrain);
        //bool b = Physics.Raycast(player.position, this.transform.forward * -1 , out hit, Vector3.Distance(transform.position,player.position));
        if (ignoreRoof) b = Physics.BoxCast(player.position + Vector3.up * 0.76f - player.transform.forward * (-.125f / 1f), new Vector3(player.lossyScale.x / 2f, player.lossyScale.y / 2f, player.lossyScale.z / 14f), pRot, out hit, transform.rotation, Vector3.Distance(transform.position, player.position + Vector3.up * 0.76f - player.transform.forward * (-.125f / 1f)), roofs);
        if (Input.GetKey(KeyCode.E))
        {
            if (!b && !ignoreRoof) print("No Hits");
            else if (b && ignoreRoof) print("Hit Terrain!");
            else if (b) print ("Hit!");
        }
        if (Input.GetKey(KeyCode.C))
        {
            StartCoroutine(ClearConsole());
        }
        if (b)
        {
            Transform objectHit = hit.transform;
            if (objectHit.gameObject.tag == "Roof")
            {
                //print("Roof Hit!");
                if (lastRoof == null || lastRoof.transform != objectHit)
                {
                    if (lastRoof != null) lastRoof.GetComponent<MeshRenderer>().material = lastRoofMat;
                    lastRoof = objectHit.GetComponent<MeshRenderer>();
                    lastRoofMat = lastRoof.material;
                    Material tMat = new Material(t_Shader);
                    tMat.name = lastRoofMat.name + "_t";
                    tMat.CopyPropertiesFromMaterial(lastRoofMat);
                    float a = defaultAlpha;
                    if (lastRoof.GetComponent<TerrainObject>() != null) a = lastRoof.GetComponent<TerrainObject>().alpha;
                    tMat.SetFloat("_alpha", a);
                    lastRoof.material = tMat;
                }
                CameraTransparency(true);
            }
            else if (objectHit.gameObject.tag == "Terrain")
            {
                //print("Terrain Hit!");
                if (lastTerrain == null || lastTerrain.transform != objectHit)
                {
                    if (lastTerrain != null) lastTerrain.GetComponent<MeshRenderer>().material = lastMat;
                    lastTerrain = objectHit.GetComponent<MeshRenderer>();
                    lastMat = lastTerrain.material;
                    Material tMat = new Material(t_Shader);
                    tMat.name = lastMat.name + "_t";
                    tMat.CopyPropertiesFromMaterial(lastTerrain.material);
                    float a = defaultAlpha;
                    if (lastTerrain.GetComponent<TerrainObject>() != null) a = lastTerrain.GetComponent<TerrainObject>().alpha;
                    tMat.SetFloat("_alpha", a);
                    lastTerrain.material = tMat;
                    if (lastTerrain.GetComponent<TerrainObject>() != null && lastTerrain.GetComponent<TerrainObject>().doChildren) foreach (Transform child in lastTerrain.transform) child.GetComponent<MeshRenderer>().material = tMat;
                }
            }
            else {
                if (lastTerrain != null) 
                {
                    lastTerrain.GetComponent<MeshRenderer>().material = lastMat;
                    if (lastTerrain.GetComponent<TerrainObject>() != null && lastTerrain.GetComponent<TerrainObject>().doChildren) foreach (Transform child in lastTerrain.transform) child.GetComponent<MeshRenderer>().material = lastMat;
                }
                if (lastRoof != null && !ignoreRoof) lastRoof.GetComponent<MeshRenderer>().material = lastRoofMat;
                lastTerrain = null;
                lastMat = null;
                if (!ignoreRoof)
                {
                    lastRoof = null;
                    lastRoofMat = null;
                }
            }
        }
        else
        {
            if (lastTerrain != null) 
            {
                lastTerrain.GetComponent<MeshRenderer>().material = lastMat;
                if (lastTerrain.GetComponent<TerrainObject>() != null && lastTerrain.GetComponent<TerrainObject>().doChildren) foreach (Transform child in lastTerrain.transform) child.GetComponent<MeshRenderer>().material = lastMat;
            }
            if (lastRoof != null && !ignoreRoof) lastRoof.GetComponent<MeshRenderer>().material = lastRoofMat;
            lastTerrain = null;
            lastMat = null;
            if (!ignoreRoof)
            {
                lastRoof = null;
                lastRoofMat = null;
            }
        }
    }

    void OnDrawGizmos ()
    {
        if(!Application.isPlaying)
            return;
        RaycastHit hit; //(transform.position - player.position).normalized
        //bool b = Physics.Raycast(transform.position, this.transform.forward , out hit, Vector3.Distance(transform.position,player.position));
        Vector3 pRot = (player.position + Vector3.up * 0.76f - transform.position).normalized * -1;
        bool b = Physics.BoxCast(player.position + Vector3.up * 0.76f - player.transform.forward * (-.125f / 1f), new Vector3(player.lossyScale.x / 2f, player.lossyScale.y / 2f, player.lossyScale.z / 14f), pRot, out hit, transform.rotation, Vector3.Distance(transform.position, player.position + Vector3.up * 0.76f - player.transform.forward * (-.125f / 1f)), terrain);
        if (b)
        {
            Gizmos.matrix = Matrix4x4.identity;
            Gizmos.color = Color.white;
            Gizmos.DrawRay(player.localPosition + Vector3.up * 0.76f - player.transform.forward * (-.125f / 1f), (player.position - player.transform.forward * (-.125f / 1f) + Vector3.up * 0.76f - transform.position).normalized * hit.distance * -1);
            Gizmos.color = Color.white;
            Gizmos.matrix = Matrix4x4.TRS(player.localPosition + Vector3.up * 0.76f - player.transform.forward * (-.125f / 1f) + (player.position + Vector3.up * 0.76f - player.transform.forward * (-.125f / 1f) - transform.position).normalized * hit.distance * -1, Quaternion.LookRotation(pRot), Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(player.lossyScale.x, player.lossyScale.y, player.lossyScale.z / 7f));
        }
        b = Physics.BoxCast(player.position + Vector3.up * 0.76f - player.transform.forward * (-.125f / 1f), new Vector3(player.lossyScale.x / 2f, player.lossyScale.y / 2f, player.lossyScale.z / 14f), pRot, out hit, transform.rotation, Vector3.Distance(transform.position, player.position + Vector3.up * 0.76f - player.transform.forward * (-.125f / 1f)), roofs);
        if (b)
        {
            Gizmos.matrix = Matrix4x4.identity;
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(player.localPosition + Vector3.up * 0.76f - player.transform.forward * (-.125f / 1f), (player.position + Vector3.up * 0.76f - player.transform.forward * (-.125f / 1f) - transform.position).normalized * hit.distance * -1);
            Gizmos.color = Color.blue;
            Gizmos.matrix = Matrix4x4.TRS(player.localPosition + Vector3.up * 0.76f - player.transform.forward * (-.125f / 1f) + (player.position + Vector3.up * 0.76f - player.transform.forward * (-.125f / 1f) - transform.position).normalized * hit.distance * -1, Quaternion.LookRotation(pRot), Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(player.lossyScale.x, player.lossyScale.y, player.lossyScale.z / 7f));
            }/*
        b = Physics.BoxCast(player.position, new Vector3(player.lossyScale.x / 2f, player.lossyScale.y / 2f, player.lossyScale.z / 14f), this.transform.forward * -1 , out hit, transform.rotation, Vector3.Distance(transform.position,player.position));
        if (b)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(player.position, transform.forward * hit.distance * -1);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(player.position + transform.forward * hit.distance * -1, player.lossyScale);
        }
        b = Physics.BoxCast(player.position, new Vector3(player.lossyScale.x / 2f, player.lossyScale.y / 2f, player.lossyScale.z / 14f), this.transform.forward * -1 , out hit, transform.rotation, Vector3.Distance(transform.position,player.position), roofs);
        if (b)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(player.position, transform.forward * hit.distance * -1);
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(player.position + transform.forward * hit.distance * -1, player.lossyScale);
        }*/
    }
}

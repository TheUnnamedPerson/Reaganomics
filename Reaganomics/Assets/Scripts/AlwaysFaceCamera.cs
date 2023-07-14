using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysFaceCamera : MonoBehaviour
{
    public Transform mainCam;
    private Vector3 scale;
    void Start ()
    {
        mainCam = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>().MainCam.transform;
        scale = transform.lossyScale;
    }
    void Update ()
    {
        //transform.LookAt(mainCam.position, Vector3.right);
        transform.rotation = Quaternion.Euler(new Vector3(mainCam.rotation.eulerAngles.x, mainCam.rotation.eulerAngles.y, 0));
        //transform.rotation = Quaternion.Euler(new Vector3(0, mainCam.rotation.eulerAngles.y + 180, 0));
        SetGlobalScale(scale);
    }
    void SetGlobalScale (Vector3 globalScale)
    {
        transform.localScale = Vector3.one;
        transform.localScale = new Vector3 (globalScale.x/transform.lossyScale.x, globalScale.y/transform.lossyScale.y, globalScale.z/transform.lossyScale.z);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif



public class CameraTrigger : MonoBehaviour
{
    [HideInInspector]
    public bool useHorizontalRotation = false;
    [HideInInspector]
    public int rotation;
    [HideInInspector]
    public int priority;
    [HideInInspector]
    public bool useVerticalRotation = false;
    [HideInInspector]
    public int verticalRot;
    void OnTriggerStay (Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (useHorizontalRotation && other.GetComponent<Character>().partyLeader == true)
            {
                other.GetComponent<PlayerMovement3D>().addRotationToQueue(rotation, priority);
            }
            if (useVerticalRotation && other.GetComponent<Character>().partyLeader == true)
            {
                other.GetComponent<PlayerMovement3D>().rotationPitch = verticalRot;
            }
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(CameraTrigger))]
public class CameraTrigger_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields

        CameraTrigger script = (CameraTrigger)target;

        // draw checkbox for the bool
        script.useHorizontalRotation = EditorGUILayout.Toggle("Change Horizontal Rotation", script.useHorizontalRotation);
        if (script.useHorizontalRotation) // if bool is true, show other fields
        {
            script.rotation = EditorGUILayout.IntSlider("Horizontal Rotation", script.rotation, 0, 2);
            script.priority = EditorGUILayout.IntField("Priority", script.priority);
        }
        script.useVerticalRotation = EditorGUILayout.Toggle("Change Verticle Rotation", script.useVerticalRotation);
        if (script.useVerticalRotation) // if bool is true, show other fields
        {
            script.verticalRot = EditorGUILayout.IntField("Verticle Rotation", script.verticalRot);
        }
    }
}
#endif

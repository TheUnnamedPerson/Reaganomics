using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCursorOnStart : MonoBehaviour
{
    public Texture2D cursor;
    void Start()
    {
        Cursor.SetCursor(cursor, new Vector2(0*-1*cursor.width/2, 0*-1*cursor.height/2), CursorMode.Auto);
    }
}

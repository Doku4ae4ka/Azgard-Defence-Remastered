using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Vector3Int screenOffset;
    private void Awake()
    {
        Vector3Int screenOffset = new Vector3Int(Screen.width/2, Screen.height/2, 0);
        transform.position = screenOffset;
    }
}

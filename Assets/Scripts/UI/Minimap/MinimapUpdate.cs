using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapUpdate : MonoBehaviour
{
    [System.Obsolete]
    public void OnTriggerEnter (Collider other)
    {
        //Debug.Log(other.name);
        GameObject parent = other.gameObject.transform.FindChild("MinimapMark").gameObject;
        if(parent != null)
        {
            parent.transform.FindChild("SpriteWhenVisible").gameObject.active = true;
            parent.transform.FindChild("SpriteWhenNotVisible").gameObject.active = false;
        }
    }
}

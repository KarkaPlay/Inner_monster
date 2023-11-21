using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapButtons : MonoBehaviour
{
    private GameObject localMapImage;
    private GameObject globalMapImage;

    private void Start ()
    {
        localMapImage = GameObject.Find("LocalMapImage");
        globalMapImage = GameObject.Find("GlobalMapImage");

        localMapImage.SetActive(true);
        globalMapImage.SetActive(false);
        Debug.Log("123");
    }

    public void _LocalMapBtn ()
    {
        Debug.Log("3333");

        localMapImage.SetActive(true);
        globalMapImage.SetActive(false);
    }

    public void _GlobalMapBtn ()
    {
        localMapImage.SetActive(false);
        globalMapImage.SetActive(true);
    }
}

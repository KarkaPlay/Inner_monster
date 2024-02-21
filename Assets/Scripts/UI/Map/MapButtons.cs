using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapButtons : MonoBehaviour
{
    public GameObject localMapImage;
    public GameObject globalMapImage;

    private void Awake ()
    {
        //localMapImage = GameObject.Find("LocalMapImage");
        //globalMapImage = GameObject.Find("GlobalMapImage");

        localMapImage.SetActive(true);
        globalMapImage.SetActive(false);
    }

    private void _LocalMapBtn ()
    {
        localMapImage.SetActive(true);
        globalMapImage.SetActive(false);
    }

    private void _GlobalMapBtn ()
    {
        localMapImage.SetActive(false);
        globalMapImage.SetActive(true);
    }
}

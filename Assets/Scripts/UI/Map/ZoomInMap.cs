using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ZoomInMap : MonoBehaviour
{
    private Camera localMapCamera;
    public GameObject localMapImage;

    [SerializeField]
    private float startSize = 100f;
    [SerializeField]
    private float minSize = 30f;
    [SerializeField]
    private float maxSize = 100f;
    [SerializeField]
    private float zoom = 10f;

    private void Start ()
    {
        localMapCamera = GetComponent<Camera>();

        if(localMapCamera.orthographic)
        {
            localMapCamera.orthographicSize = startSize;
        }
    }
    private void Update ()
    {
        Zoom();
    }

    private void Zoom ()
    {
        if(localMapCamera.orthographic && localMapImage.active)
        {
            if(Input.GetAxis("Mouse ScrollWheel") > 0 && localMapCamera.orthographicSize >= minSize)
            {
                localMapCamera.orthographicSize -= 0.1f * zoom;
                MoveCamera();
            }
            else
                if(Input.GetAxis("Mouse ScrollWheel") < 0 && localMapCamera.orthographicSize <= maxSize)
            {
                localMapCamera.orthographicSize += 0.1f * zoom;
                MoveCamera();
            }
        }
        if(localMapCamera.orthographic && (Input.GetKey(KeyCode.M) || !localMapImage.active))
        {
            localMapCamera.orthographicSize = startSize;
            transform.localPosition = new Vector3(0, 0, -10);
        }
    }

    private void MoveCamera ()
    {
        Vector3 mousePosition = localMapCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        if(transform.localPosition.x + 0.2f <= 3 && mousePosition.x > 0)
        {
            transform.localPosition += new Vector3(mousePosition.x / 100, 0, 0);
        }
        if(transform.localPosition.x - 0.2f >= -3 && mousePosition.x < 0)
        {
            transform.localPosition += new Vector3(mousePosition.x / 100, 0, 0);
        }
        if(transform.localPosition.y + 0.2f <= 4 && mousePosition.z > 0)
        {
            transform.localPosition += new Vector3(0, mousePosition.z / 100, 0);
        }
        if(transform.localPosition.y - 0.2f >= -4 && mousePosition.z < 0)
        {
            transform.localPosition += new Vector3(0, mousePosition.z / 100, 0);
        }
    }
}
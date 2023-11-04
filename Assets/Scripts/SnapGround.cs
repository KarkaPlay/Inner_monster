/*using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SnapToGround : MonoBehaviour
{
    [MenuItem("Custom/Snap To Ground %g")]
    public static void Ground()
    {
        foreach(var transform in Selection.transforms)
        {
            var hits = Physics.RaycastAll(transform.position + Vector3.up, Vector3.down, 10f);
            foreach(var hit in hits)
            {
                if (hit.collider.gameObject == transform.gameObject)
                    continue;

                transform.position = hit.point;
                break;
            }
        }
    }
public static void SnaptoGround()
    {
        //Looping through each of the selected Transforms.
        foreach(Transform t in Selection.transforms)
        {
            //These lists will hold all children colliders and the respective layers, so that we can set them back to their respective layers.
            List<int> childrenLayers = new List<int>();
            List<Collider> children = t.GetComponentsInChildren<Collider>().ToList();

            //Getting all the current layers of the children.
            foreach (Collider c in children)
            {
                childrenLayers.Add(c.gameObject.layer);

                //Setting ignore raycast.
                c.gameObject.layer = 2;
            }

            //Getting the mesh Renderers for all children.
            MeshRenderer[] renderers = t.GetComponentsInChildren<MeshRenderer>();

            //This is to store the children object which has the lowest bound related to y axis.
            Bounds? lowerBounds = null;
            Transform lowerTransform = null;
            float minY = Mathf.Infinity;

            //Looping through the children mr to find the lowest.
            foreach (MeshRenderer mr in renderers)
            {
                //Getting current bounds.
                Bounds b = mr.bounds;

                //If the center - extents of the current mesh renderer is the lowest, then storing the data.
                if (b.center.y - b.extents.y < minY)
                {
                    lowerBounds = b;
                    minY = b.center.y - b.extents.y;
                    lowerTransform = mr.transform;
                }
            }

            //To store the raycast hit.
            RaycastHit hit;

            //Since we now know which children has the lowest center + extents, raycasting from him.
            if (Physics.Raycast(lowerTransform.position + Vector3.up, Vector3.down, out hit, 50))
            {

                //Just snapping based on the extentes of the mesh, if any lower bounds is found
                //This actually can only not happen if no children object, or the parent, has any mesh renderer.
                if (lowerBounds != null)
                {
                    //Setting the position to be then: Using the x and z values of the original selected transform, because:
                    //If the parent transform is not aligned with the children objects, this certifies that the selected object 
                    //does not move to the position x,z of the center of the child which the raycast got cast.
                    //Then the y value is defined by the point hit + the difference from the center of the child that cast the raycast and
                    //the y of the parent object + the extents of the lowest bounds.
                    t.position = new Vector3(t.position.x,
                        hit.point.y + (t.position.y - lowerTransform.position.y) + lowerBounds.Value.extents.y,
                        t.position.z);
                }
                else
                {
                    //Setting the position of the selected object to be the point hit.
                    t.position = new Vector3 (t.position.x, hit.point.y);
                }

            }

            int i = 0;
            //Goingo through each collider of the children and re setting the layer values back.
            foreach (Collider c in children)
            {
                c.gameObject.layer = childrenLayers[i];
                i++;
            }
        }
    }


}*/
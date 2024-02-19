using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionFinder : MonoBehaviour
{
    
    [SerializeField]
    public static List<Interactable> m_NearbyInteractables = new List<Interactable>();
    
    public bool HasNearbyInteractables()
    {
        
        return m_NearbyInteractables.Count != 0;
        
    }

    private void Update()
    {
        if (HasNearbyInteractables() && Input.GetKeyDown(KeyCode.E))
        {
            //Ideally, we'd want to find the best possible interaction (ex: by distance & orientation).
            
            m_NearbyInteractables[0].DoInteraction();
  
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable != null)
        {
            m_NearbyInteractables.Add(interactable);
           

        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable != null)
        {
            m_NearbyInteractables.Remove(interactable);

        }
        
    }
 
}
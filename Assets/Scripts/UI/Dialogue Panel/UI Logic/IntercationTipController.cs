using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntercationTipController : MonoBehaviour
{
    [SerializeField]
    private InteractionFinder m_InteractionFinder;
    [SerializeField]
    private Image m_Image;

    void Update()
    {
        m_Image.enabled = m_InteractionFinder.HasNearbyInteractables();
    }
}
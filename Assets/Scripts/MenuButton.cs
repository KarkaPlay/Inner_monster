using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    private RectTransform rectTransform;
    public AudioSource audioSource;
    
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(ScaleUp());
        audioSource.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartCoroutine(ScaleDown());
    }
    
    // Корутина для анимации увеличения кнопки
    IEnumerator ScaleUp()
    {
        StopCoroutine(ScaleDown());
        while (rectTransform.localScale.x < 1.1f)
        {
            rectTransform.localScale += new Vector3(0.02f, 0.02f, 0.02f);
            yield return new WaitForSeconds(0.01f);
        }
    }
    
    // Корутина для анимации уменьшения кнопки
    IEnumerator ScaleDown()
    {
        StopCoroutine(ScaleUp());
        while (rectTransform.localScale.x > 1f)
        {
            rectTransform.localScale -= new Vector3(0.02f, 0.02f, 0.02f);
            yield return new WaitForSeconds(0.01f);
        }
    }
}

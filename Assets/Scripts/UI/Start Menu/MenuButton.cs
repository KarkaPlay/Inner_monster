using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IPointerUpHandler, IPointerDownHandler
{
    
    public AudioSource audioSource;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        audioSource.Play();
        _animator.SetFloat("Highlited", 1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _animator.SetFloat("Highlited", -1);
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        _animator.SetFloat("Pressed", 1);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        _animator.SetFloat("Pressed", -1);
    }
   
}

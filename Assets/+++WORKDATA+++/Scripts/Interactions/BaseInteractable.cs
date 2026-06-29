using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseInteractable : MonoBehaviour
{
    public UnityEvent OnInteracted;
    public UnityEvent OnHovered;
    public UnityEvent OnUnhovered;
    
    public bool canInteract = true;
    
    protected bool isHovered = false;

    protected virtual void OnDisable()
    {
        Unhover();
    }

    public virtual void Interact()
    {
        Debug.Log("Interacted");
        OnInteracted?.Invoke();
    }

    public virtual void Hover()
    {
        if (isHovered) return;
        
        isHovered = true;
        OnHovered?.Invoke();
    }

    public virtual void Unhover()
    {
        if(!isHovered) return;
        isHovered = false;
        OnUnhovered?.Invoke();
    }
}
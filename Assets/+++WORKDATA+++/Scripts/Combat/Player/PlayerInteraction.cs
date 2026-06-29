using System;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public static Action<BaseInteractable> SetInteraction;
    public static Action OnInteract;
    private BaseInteractable currentInteractable;

    private void OnEnable()
    {
        SetInteraction += SetInteractable;
        OnInteract += Interact;
    }

    private void OnDisable()
    {
        SetInteraction -= SetInteractable;
        OnInteract -= Interact;
    }

    private void SetInteractable(BaseInteractable baseInteractable)
    {
        currentInteractable = baseInteractable;
    }

    private void Interact()
    {
        if (!currentInteractable) return;
        
        currentInteractable.Interact();
    }
}

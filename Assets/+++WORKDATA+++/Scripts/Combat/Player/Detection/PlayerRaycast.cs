using System;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    [SerializeField] private float rayDistance = 5f;
    [SerializeField] private Transform rayOrigin;

    private BaseInteractable _currentInteractable;
    
    private void Update()
    {
        HandleRaycast();
    }

    private void LateUpdate()
    {
        Debug.DrawRay(rayOrigin.position, rayOrigin.forward * rayDistance, 
            _currentInteractable? Color.green :Color.red);
    }

    private void HandleRaycast()
    {
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.TryGetComponent(out BaseInteractable interactable)) 
            {
                _currentInteractable = interactable;
                _currentInteractable.Hover();
                PlayerInteraction.SetInteraction(_currentInteractable);
                Debug.Log(_currentInteractable);
                return;
            }
        }

        if (!_currentInteractable) return;
        
        _currentInteractable.Unhover();
        _currentInteractable = null;
        PlayerInteraction.SetInteraction(null);
    }
}
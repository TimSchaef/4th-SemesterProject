using System.Collections;
using UnityEngine;

public class MultiInteractable : BaseInteractable
{
    [SerializeField] private float timeForNextInteraction = 2f;
    private bool _isStillHovered;

    public override void Interact()
    {
        if (!canInteract) return;

        canInteract = false;
        StartCoroutine(WaitForInteraction());
        base.Unhover();
        base.Interact();
    }

    public override void Hover()
    {
        _isStillHovered = true;
        
        if (!canInteract) return;
        base.Hover();
    }

    public override void Unhover()
    {
        _isStillHovered = false;

        if (!canInteract) return;
        base.Unhover();
    }

    IEnumerator WaitForInteraction()
    {
        yield return new WaitForSeconds(timeForNextInteraction);
        canInteract = true;

        if (_isStillHovered)
            base.Hover();

    }
}

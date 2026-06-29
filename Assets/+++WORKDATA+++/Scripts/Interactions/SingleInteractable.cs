
public class SingleInteractable : BaseInteractable
{
    public override void Interact()
    {
        if (!canInteract) return;
        
        canInteract = false;
        base.Interact();
        base.Unhover();
    }

    public override void Hover()
    {
        if (!canInteract) return;
        
        base.Hover();
    }
}

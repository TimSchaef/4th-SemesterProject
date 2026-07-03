using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        animator.SetTrigger("isOpen");
    }

    public void CloseDoor()
    {
        animator.SetTrigger("isClosed");
    }
}

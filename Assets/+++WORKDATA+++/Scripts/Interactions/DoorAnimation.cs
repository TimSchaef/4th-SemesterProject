using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    void Awake()
    {
        if (animator != null)
            animator = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        print("Open door");
        animator.SetTrigger("isOpen");
    }

    public void CloseDoor()
    {
        animator.SetTrigger("isClosed");
    }
}

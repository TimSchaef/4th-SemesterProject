using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    void Awake()
    {
            animator = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        print("Open door");
        animator.SetTrigger("isOpen");
    }

    public void CloseDoor()
    {
        print("Close door");
        animator.SetTrigger("isClosed");
    }
}

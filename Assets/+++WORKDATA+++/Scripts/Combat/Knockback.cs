using UnityEngine;

public class Knockback : MonoBehaviour
{
    [SerializeField] private float damping = 8f;

    private Vector3 velocity;
    
    public Vector3 Velocity => velocity;

    public void AddKnockback(Vector3 force)
    {
        velocity += force;
    }
    void Tick()
    {
        if (velocity.magnitude > 0.2f)
        {
            transform.position += velocity * Time.deltaTime;
            
            velocity = Vector3.Lerp(velocity, Vector3.zero, damping * Time.deltaTime);
        }
    }
}

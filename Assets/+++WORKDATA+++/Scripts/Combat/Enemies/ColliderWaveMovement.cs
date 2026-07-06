using UnityEngine;

public class ColliderWaveMovement: MonoBehaviour
{
    public float speed = 2.0f;
    public float amplitude = 0.5f;
    
    private SphereCollider sphereCollider;
    private float baseRadius;

    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        baseRadius = sphereCollider.radius;
    }

    void Update()
    {
        // Berechne exakt den gleichen Wert wie im Shader Graph
        float wave = Mathf.Sin(Time.time * speed) * amplitude;
        
        // Radius des Colliders anpassen
        sphereCollider.radius = baseRadius + wave;
    }
}
using UnityEngine;

public class XPCollector : MonoBehaviour
{
    [SerializeField] private float collectRadius = 3f;
    
    public float CollectRadius => collectRadius;

    public void IncreaseRadius(float amount)
    {
        collectRadius += amount;
    }
}

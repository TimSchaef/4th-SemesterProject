using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public static DamageManager Instance;

    [SerializeField] private DamagePopup popupPrefab;
    [SerializeField] private int poolSize = 50;
    
    private Queue<DamagePopup> pool = new();

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < poolSize; i++)
        {
            var popup = Instantiate(popupPrefab, transform);
            popup.gameObject.SetActive(false);
            
            pool.Enqueue(popup);
        }
    }

    public void Show(float damage, Vector3 position)
    {
        DamagePopup popup = pool.Dequeue();
        popup.transform.position = position;
        popup.gameObject.SetActive(true);
        popup.Show(damage);
    }

    public void Return(DamagePopup popup)
    {
        popup.gameObject.SetActive(false);
        pool.Enqueue(popup);
    }
}

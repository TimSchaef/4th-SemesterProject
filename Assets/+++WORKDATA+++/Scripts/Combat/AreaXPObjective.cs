using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AreaXPObjective : MonoBehaviour
{
    [SerializeField] public float requiredTime = 10;
    [SerializeField] private float xpMultiplier = 0.3f;
    [SerializeField] private Renderer renderer;
    [SerializeField] private TextMeshProUGUI currentTimeTMP;

    private float currentTime = 0;
    private bool isCaptured;
    private Collider areaCollider;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTimeTMP.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay (Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Counter();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CounterDown();
        }
    }
    private void Counter()
    {
        if (isCaptured) return;
        
        currentTime += Time.deltaTime;
        currentTimeTMP.gameObject.SetActive(true);
        currentTimeTMP.text = currentTime.ToString("0.00");

        if (currentTime >= requiredTime)
        {
            currentTimeTMP.gameObject.SetActive(false);
            PlayerXP.Instance.xpMultiplier += xpMultiplier;
            isCaptured = true;
            renderer.material.color = Color.green;
            StartCoroutine(DestroyArea());
        }
    }

    private IEnumerator DestroyArea()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    private void CounterDown()
    {
        if (isCaptured) return;

        if (currentTime >= 0)
        {
            currentTime = 0;
        }
    }
}

using UnityEngine;

public class XPPickup : MonoBehaviour
{
    private float _xpAmount;
    private Transform _player;
    private XPCollector _collector;

    private bool _inRange;
    private bool _collected;

    [SerializeField] private float collectSpeed = 15f;

    public void Initialize(SO_ExperiencePoints xpData)
    {
        _xpAmount = xpData.xpAmount;
        
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _collector = _player.GetComponent<XPCollector>();
        
        GetComponentInChildren<Renderer>().material = xpData.xpMaterial;
    }

    private void Update()
    {
        if (_player == null)
            return;
        
        float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);

        if (distanceToPlayer < _collector.CollectRadius)
            _inRange = true;

        if (_inRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position,
                collectSpeed * Time.deltaTime);
        }

        if (distanceToPlayer <= 0.5f)
        {
            Collect();
        }
    }

    private void Collect()
    {
        _collected = true;
        
        PlayerXP.Instance.AddXP((int)_xpAmount);
        Destroy(gameObject);

    }
}

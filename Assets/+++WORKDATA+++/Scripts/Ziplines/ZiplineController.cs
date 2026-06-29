using UnityEngine;

public class ZiplineController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private PlayerStateHandler stateHandler;
    [SerializeField] private PlayerMovement movement;

    private Zipline _currentZipline;
    private float _positionProgress;
    private bool _active;

    [Header("Settings")]
    [SerializeField] private float speed = 2f;

    void Update()
    {
        if (!_active || _currentZipline == null) return;

        HandleMovement();
    }

    public void EnterZipline(Zipline zipline)
    {
        _currentZipline = zipline;
        _positionProgress = 0f;
        _active = true;
        
        stateHandler.SetState(PlayerStateHandler.PlayerState.OnZipline);
    }

    public void ExitZipline()
    {
        _active = false;
        _currentZipline = null;

        stateHandler.SetState(PlayerStateHandler.PlayerState.Default);
    }

    private void HandleMovement()
    {
        _positionProgress += Time.deltaTime * speed;

        if (_positionProgress >= 1f)
        {
            ExitZipline();
            return;
        }

        Vector3 pos = Vector3.Lerp(
            _currentZipline.startPoint.position,
            _currentZipline.endPoint.position,
            _positionProgress
        );

        player.position = pos;
    }
}

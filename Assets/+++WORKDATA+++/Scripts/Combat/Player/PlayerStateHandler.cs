using UnityEngine;

public class PlayerStateHandler : MonoBehaviour
{
    public static PlayerStateHandler Instance;
    public enum PlayerState
    {
        Default,
        OnZipline,
        InShop
    }
    public PlayerState CurrentState { get; private set; } = PlayerState.Default;

    public void SetState(PlayerState newState)
    {
        CurrentState = newState;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}

using UnityEngine;
public class PlayerStateModel : MonoBehaviour
{
    public PlayerState CurrentPlayerState { get; private set; }

    public delegate void Eventhandler(PlayerState state);
    public event Eventhandler OnStateChanged;

    public void UpdateState(PlayerState state)
    {
        CurrentPlayerState = state;
        Debug.Log($"is changing state to: {state}");
        OnStateChanged?.Invoke(state);
    }
}
using System.Linq;

using UnityEngine;

public class BotModel : MonoBehaviour
{
    public delegate void VisitEventhandler(Vector3 location, bool immediate);
    public event VisitEventhandler OnGoToPlanet;
    public event VisitEventhandler OnGoToPosition;

    public delegate void EventHandler();
    public event EventHandler OnBackToPlayer;

    public BotState CurrentBotState { get; private set; }
    public StateIconPreset[] StatePresets;
    
    public delegate void StateHandler();
    public event StateHandler OnBotStateChanged;

    public StateIconPreset CurrentStateInfo()
    {
        return StatePresets.Single(item => item.State == CurrentBotState);
    }
    
    internal void UpdateBotState(BotState state)
    {
        CurrentBotState = state;
        OnBotStateChanged?.Invoke();
    }
    
    internal void GoToPlanet(Vector3 location)
    {
        UpdateBotState(BotState.Visit);
        OnGoToPlanet?.Invoke(location, false);
    }

    internal void GoToPosition(Vector3 location, bool immediate)
    {
        OnGoToPosition?.Invoke(location, immediate);
    }

    internal void BackToPlayer()
    {
        OnBackToPlayer?.Invoke();
    }
}
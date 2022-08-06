using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameModel : MonoBehaviour
{
    [SerializeField]
    private float _timeLimit;
    
    [HideInInspector]
    public float SecondsLeft;
    
    public MinigameType ActiveMinigame { get; private set; }

    public delegate void Eventhandler();
    public event Eventhandler OnMinigameStart;
    public event Eventhandler OnMinigameComplete;

    internal void StartMinigame()
    {
        OnMinigameStart?.Invoke();
    }

    internal void CompleteMinigame()
    {
        SecondsLeft = 0;
        OnMinigameComplete?.Invoke();
    }
    
    public void StartNewMinigame(MinigameType type)
    {
        ActiveMinigame = type;
        StartMinigame();
        SecondsLeft = _timeLimit;
    }
}

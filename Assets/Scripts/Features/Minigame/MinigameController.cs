using System;
using UnityEngine;

public class MinigameController : MonoBehaviour
{
    public MinigameModel MinigameModel;
    public PlayerStateModel PlayerStateModel;

    public void StartMinigame(MinigameType type)
    {
        MinigameModel.StartNewMinigame(type);
        PlayerStateModel.UpdateState(PlayerState.Minigame);
    }

    void Update()
    {
        if (MinigameModel.SecondsLeft > 0)
        {
            MinigameModel.SecondsLeft -= Time.deltaTime;
        }
        else if (MinigameModel.SecondsLeft < 0)
        {
            MinigameModel.CompleteMinigame();
        }
    }
}


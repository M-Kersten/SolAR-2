using System;

using UnityEngine;
public class PlayerStateController : MonoBehaviour
{
    public PlayerStateModel PlayerStateModel;
    public TutorialModel TutorialModel;
    
    private void Awake()
    {
        TutorialModel.OnTutorialFinished += TutorialFinished;
    }

    private void Start()
    {
        bool tutorialSeen = TutorialModel.TutorialSeen;
        PlayerStateModel.UpdateState(tutorialSeen ? PlayerState.Intro : PlayerState.Tutorial);
    }

    private void OnDestroy()
    {
        TutorialModel.OnTutorialFinished -= TutorialFinished;
    }

    public void TutorialFinished()
    {
        PlayerStateModel.UpdateState(PlayerState.Intro);
    }
}
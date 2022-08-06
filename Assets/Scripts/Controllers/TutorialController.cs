using System;

using UnityEngine;


public class TutorialController: MonoBehaviour
{
    public TutorialModel TutorialModel;
    public PlayerStateModel PlayerStateModel;
    public FaderModel FaderModel;

    private void Awake()
    {
        PlayerStateModel.OnStateChanged += CheckForTutorialState;
        TutorialModel.UpdateTutorialSeen(PlayerPrefs.HasKey(TutorialModel.TutorialKey) && PlayerPrefs.GetInt(TutorialModel.TutorialKey) == 1);
    }

    private void Start()
    {
        PlayerStateModel.UpdateState(TutorialModel.TutorialSeen ? PlayerState.Intro : PlayerState.Tutorial);
    }

    void CheckForTutorialState(PlayerState state)
    {
        if (state == PlayerState.Tutorial)
            StartTutorial();
    }

    public void StartTutorial()
    {
        TutorialModel.StartTutorial();
    }
    
    public void EndTutorial()
    {
        FaderModel.OnFullyFaded += FinishTutorial;
        FaderModel.StartFading();
    }

    void FinishTutorial()
    {
        FaderModel.OnFullyFaded -= FinishTutorial;
        PlayerPrefs.SetInt(TutorialModel.TutorialKey, 1);
        TutorialModel.UpdateTutorialSeen(true);
        TutorialModel.FinishTutorial();
    }
    
    public void NextTutorialStep()
    {
        TutorialModel.ActiveTutorialStep++;
        if (TutorialModel.TutorialDialogs.Length <= TutorialModel.ActiveTutorialStep)
        {
            TutorialModel.AllStepsCompleted();
        }
        else
        {
            TutorialModel.NextTutorialStep();
        }
    }
}

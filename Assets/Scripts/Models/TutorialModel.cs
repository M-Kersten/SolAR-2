using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TutorialModel : MonoBehaviour
{
    public TutorialDialog[] TutorialDialogs;

    public bool DebugTutorial;
    public int ActiveTutorialStep;
    public bool TutorialSeen;
    public readonly string TutorialKey = "TutorialSeen";
    
    public delegate void Eventhandler();
    public event Eventhandler OnTutorialStart;
    public event Eventhandler OnNextTutorialStep;
    public event Eventhandler OnAllStepsCompleted;
    public event Eventhandler OnTutorialFinished;

    internal void UpdateTutorialSeen(bool tutorialSeen)
    {
        //#if UNITY_EDITOR
        if (DebugTutorial)
        {
            TutorialSeen = false;
            return;
        }
       // #endif
        TutorialSeen = tutorialSeen;
    }
    
    internal void StartTutorial()
    {
        OnTutorialStart?.Invoke();
    }

    internal void AllStepsCompleted()
    {
        OnAllStepsCompleted?.Invoke();
    }
    
    internal void NextTutorialStep()
    {
        OnNextTutorialStep?.Invoke();
    }

    internal void FinishTutorial()
    {
        OnTutorialFinished?.Invoke();
    }

    public TutorialDialog CurrentTutorial
    {
        get
        {
            return TutorialDialogs[ActiveTutorialStep];
        }
    }
}

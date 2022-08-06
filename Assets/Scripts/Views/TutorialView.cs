using System;

using UnityEngine;
public class TutorialView : MonoBehaviour
{
    [SerializeField]
    private AnimatedText _tutorialText;
    [SerializeField]
    private Camera _tutorialCamera;
    [SerializeField]
    private Canvas _tutorialCanvas;
    
    private TutorialModel _tutorialModel;

    void Awake()
    {
        _tutorialModel = FindObjectOfType<TutorialModel>();
        _tutorialModel.OnTutorialStart += StartTutorial;
        _tutorialModel.OnNextTutorialStep += NextTutorial;
        _tutorialModel.OnTutorialFinished += CompleteTutorial;
        
        _tutorialText.SetText(string.Empty);
        _tutorialCamera.gameObject.SetActive(false);
        _tutorialCanvas.enabled = false;
    }

    void StartTutorial()
    {
        var currentTutorialInfo = _tutorialModel.CurrentTutorial;
        _tutorialText.SetText(currentTutorialInfo.Text);
        _tutorialCamera.gameObject.SetActive(true);
        _tutorialCanvas.enabled = true;
    }

    void NextTutorial()
    {
        var currentTutorialInfo = _tutorialModel.CurrentTutorial;
        _tutorialText.SetText(currentTutorialInfo.Text);
    }

    void CompleteTutorial()
    {
        _tutorialCamera.gameObject.SetActive(false);
        _tutorialCanvas.enabled = false;
    }
}
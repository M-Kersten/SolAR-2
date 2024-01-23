using UnityEngine;
public class FaderModel: MonoBehaviour
{
    [SerializeField]
    private float fadeDuration;
    
    public delegate void Eventhandler();
    public event Eventhandler OnFadeStart;
    public event Eventhandler OnFullyFaded;
    public event Eventhandler OnFadeComplete;

    public float FadeDuration { get => fadeDuration; private set => fadeDuration = value; }
    
    public void StartFading()
    {
        OnFadeStart?.Invoke();
    }

    public void FullFaded()
    {
        OnFullyFaded?.Invoke();
    }

    public void CompleteFade()
    {
        OnFadeComplete?.Invoke();
    }
}
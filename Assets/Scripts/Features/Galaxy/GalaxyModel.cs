using Niantic.ARDK.Extensions;

using UnityEngine;


public class GalaxyModel: MonoBehaviour
{
    public delegate void PivotEventhandler(Vector3 scalePivot);
    public delegate void Eventhandler();
    public event PivotEventhandler OnZoomInStart;
    public event PivotEventhandler OnZoomOutStart;
    public event Eventhandler OnZoomInComplete;
    public event Eventhandler OnZoomOutComplete;
    public event Eventhandler OnStartGame;
    public event Eventhandler OnUncoupleStar;

    public ARRenderingManager RenderingManager;
    public float ScaleAmount;
    public float ZoomInDuration;
    public GalaxyButtonType SelectedGalaxyButtonType;

    internal void StartGame()
    {
        OnStartGame?.Invoke();
    }
    
    public void StartZooming(Vector3 value)
    {
        OnZoomInStart?.Invoke(value);
    }

    public void StartZoomingOut(Vector3 value)
    {
        OnZoomOutStart?.Invoke(value);
    }

    public void FinishZoomingIn()
    {
        OnZoomInComplete?.Invoke();
    }

    public void FinishZoomingOut()
    {
        OnZoomOutComplete?.Invoke();
    }

    internal void UnCoupleStar()
    {
        OnUncoupleStar?.Invoke();
    }
}
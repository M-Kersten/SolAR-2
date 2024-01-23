using Niantic.ARDK.Extensions;
using Niantic.ARDK.Templates;

using UnityEngine;

public class PlacementModel: MonoBehaviour
{
    public SolarPlacementView SolarSystem;
    
    public float SpawnDistance;
    public Vector2 MinMaxScale;
    public ARRenderingManager RenderingManager;
    public Camera ArCamera;

    public delegate void Eventhandler();
    public event Eventhandler OnSolarSystemPlaced;
    public event Eventhandler OnSolarSystemMoved;

    internal void MovedSolarSystem()
    {
        OnSolarSystemMoved?.Invoke();
    }
    
    internal void PlaceSolarSystem()
    {
        OnSolarSystemPlaced?.Invoke();
    }
}

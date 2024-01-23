using System.Linq;

using UnityEngine;

public class PlanetModel: MonoBehaviour
{
    public Material SpaceSkybox;
    public PlanetInfo[] PlanetInfos;

    public readonly string VisitorColliderTag = "VisitCollider";
    
    public delegate void Eventhandler(Transform position);
    public event Eventhandler OnSpawnedOnPlanet;
    
    public PlanetInfo CurrentPlanetInfo { get; private set; }

    internal void SpawnOnPlanet(Transform position)
    {
        OnSpawnedOnPlanet?.Invoke(position);
    }
    
    public void UpdatePlanetInfo(PlanetName name)
    {
        CurrentPlanetInfo = PlanetInfos.Single(item => item.Name == name);
    }
}
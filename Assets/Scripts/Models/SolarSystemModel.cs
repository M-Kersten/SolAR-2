using UnityEngine;

public class SolarSystemModel: MonoBehaviour
{
    public delegate void Eventhandler();
    public event Eventhandler OnSolarSystemSpawned;

    internal void OnSpawnSolarSystem()
    {
        OnSolarSystemSpawned?.Invoke();
    }
}
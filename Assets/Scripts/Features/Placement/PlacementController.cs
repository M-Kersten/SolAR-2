using System;

using UnityEngine;
public class PlacementController : MonoBehaviour
{
    public PlacementModel PlacementModel;
    public FaderModel FaderModel;
        
    private void Awake()
    {
        PlacementModel.OnSolarSystemPlaced += InitSolarSystem;
    }

    private void OnDestroy()
    {
        PlacementModel.OnSolarSystemPlaced -= InitSolarSystem;
    }

    public void PlaceSolarSystem()
    {
        PlacementModel.PlaceSolarSystem();
    }

    void InitSolarSystem()
    {
        FaderModel.StartFading();
    }
}
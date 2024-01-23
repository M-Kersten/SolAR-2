using System;
using UnityEngine;
using Niantic.ARDK.Extensions;

public class SolarPlaneView : MonoBehaviour
{
    public ARPlaneManager PlaneManager;

    private GalaxyModel _galaxyModel;
    private PlacementModel _placementModel;
    
    private void Start()
    {
        _galaxyModel = FindObjectOfType<GalaxyModel>();
        if (_galaxyModel)
        {
            PlaneManager.enabled = false;
            _galaxyModel.OnStartGame += SetPlanesActive;
        }

        _placementModel = FindObjectOfType<PlacementModel>();
        if (_placementModel)
        {
            _placementModel.OnSolarSystemPlaced += SetPlanesInactive;
        }
        
        SetPlanesInactive();
    }

    private void OnDestroy()
    {
        if (_galaxyModel)
            _galaxyModel.OnStartGame -= SetPlanesActive;
        
        if (_placementModel)
            _placementModel.OnSolarSystemPlaced -= SetPlanesInactive;
    }

    void SetPlanesActive()
    {
        PlaneManager.enabled = true;
    }

    void SetPlanesInactive()
    {
        PlaneManager.ClearAllPlanes();
        PlaneManager.enabled = false;
    }
}

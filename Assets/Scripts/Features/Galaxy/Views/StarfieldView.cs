using System;
using UnityEngine;

public class StarfieldView: MonoBehaviour
{
    public GameObject Starfield;
    
    private GalaxyModel _galaxyModel;
    private PlacementModel _placementModel;

    private void Start()
    {
        _galaxyModel = FindObjectOfType<GalaxyModel>();
        if (_galaxyModel)
            _galaxyModel.OnStartGame += DisableStarfield;

        _placementModel = FindObjectOfType<PlacementModel>();
        if (_placementModel)
            _placementModel.OnSolarSystemPlaced += EnableStarfield;
    }

    private void OnDestroy()
    {
        if (_galaxyModel)
            _galaxyModel.OnStartGame -= DisableStarfield;

        if (_placementModel)
            _placementModel.OnSolarSystemPlaced -= EnableStarfield;
    }

    void EnableStarfield()
    {
        Starfield.SetActive(true);
    }
    
    void DisableStarfield()
    {
        Starfield.SetActive(false);
    }
}
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlacementCanvasView: MonoBehaviour
{
    public CanvasGroup CanvasGroup;
    public Button PlaceButton;
    
    private GalaxyModel _galaxyModel;
    private PlacementModel _placementModel;

    private void Start()
    {
        _galaxyModel = FindObjectOfType<GalaxyModel>();
        if (_galaxyModel)
            _galaxyModel.OnStartGame += EnableCanvas;

        _placementModel = FindObjectOfType<PlacementModel>();
        if (_placementModel)
        {
            _placementModel.OnSolarSystemPlaced += DisableCanvas;
            _placementModel.OnSolarSystemMoved += EnablePlaceButton;
        }
        
        DisableCanvas();
        PlaceButton.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (_galaxyModel)
            _galaxyModel.OnStartGame -= EnableCanvas;

        if (_placementModel)
        {
            _placementModel.OnSolarSystemPlaced -= DisableCanvas;
            _placementModel.OnSolarSystemMoved -= EnablePlaceButton;
        }
    }

    void EnablePlaceButton()
    {
        PlaceButton.gameObject.SetActive(true);
    }

    void EnableCanvas()
    {
        CanvasGroup.interactable = true;
        CanvasGroup.alpha = 1;
    }
    
    void DisableCanvas()
    {
        CanvasGroup.interactable = false;
        CanvasGroup.alpha = 0;
    }
}
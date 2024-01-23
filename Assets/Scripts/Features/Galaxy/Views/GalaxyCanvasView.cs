using System;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class GalaxyCanvasView: MonoBehaviour
{
    public CanvasGroup CanvasGroup;
    
    private GalaxyModel _galaxyModel;
    
    private void Awake()
    {
        _galaxyModel = FindObjectOfType<GalaxyModel>();
        _galaxyModel.OnZoomInStart += OnZoomInStart;
        _galaxyModel.OnZoomOutComplete += EnableCanvas;
    }

    private void OnDestroy()
    {
        _galaxyModel.OnZoomOutComplete -= EnableCanvas;
        _galaxyModel.OnZoomInStart -= OnZoomInStart;
    }

    void OnZoomInStart(Vector3 value)
    {
        DisableCanvas();
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
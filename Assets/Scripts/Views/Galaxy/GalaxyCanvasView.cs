using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class GalaxyCanvasView: MonoBehaviour
{
    public CanvasGroup CanvasGroup;
    
    private GalaxyModel _galaxyModel;
    
    private void Awake()
    {
        _galaxyModel = FindObjectOfType<GalaxyModel>();
        if (_galaxyModel)
        {
            _galaxyModel.OnZoomInStart += value => DisableCanvas();
            _galaxyModel.OnZoomOutComplete += EnableCanvas;
        }
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
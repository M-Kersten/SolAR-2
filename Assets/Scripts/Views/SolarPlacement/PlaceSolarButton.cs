using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PlaceSolarButton: MonoBehaviour
{
    private SolarPlacementController _solarPlacementController;
    private FaderModel _faderModel;
    
    private void Start()
    {
        _solarPlacementController = FindObjectOfType<SolarPlacementController>();
        _faderModel = FindObjectOfType<FaderModel>();
        
        GetComponent<Button>().onClick.AddListener(Place);
    }

    void Place()
    {
        _solarPlacementController.SetPlacingActive(false);
        _faderModel.StartFading();
        _faderModel.OnFullyFaded += _solarPlacementController.PlaceSolar;
    }
}
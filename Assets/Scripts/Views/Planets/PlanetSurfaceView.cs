using System;

using UnityEngine;

public class PlanetSurfaceView : MonoBehaviour
{
    private DialogController _dialogController;
    private BotController _botController;
    private PlanetModel _planetModel;

    public Transform SpawnPosition;

    private void Start()
    {
        _dialogController = FindObjectOfType<DialogController>();
        _botController = FindObjectOfType<BotController>();
        _planetModel = FindObjectOfType<PlanetModel>();
        
        _planetModel.SpawnOnPlanet(SpawnPosition);
        _botController.FlyToSurfacePosition(SpawnPosition);
        _dialogController.Talk(_planetModel.CurrentPlanetInfo.PlanetWelcomeDialog);
    }
    
    
    
    
}
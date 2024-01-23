using System;

using UnityEngine;

public class BotController : MonoBehaviour
{
    public BotModel BotModel;
    public PlanetModel PlanetModel;
    
    private void Start()
    {
        BotModel.UpdateBotState(BotState.Idle);
    }

    public void FlyToPlanet(Vector3 location)
    {
        BotModel.GoToPlanet(location);
    }

    public void FlyToSurfacePosition(Transform spawnPosition)
    {
        BotModel.UpdateBotState(BotState.PlanetIdle);
        var cameraTransform = GameObject.FindWithTag("MainCamera").transform;
        var lockPosition = spawnPosition.position + cameraTransform.forward;
        lockPosition = new Vector3(lockPosition.x, spawnPosition.position.y - .15f, lockPosition.z);
        BotModel.GoToPosition(lockPosition, true);
    }

    public void BackToPlayer()
    {
        BotModel.BackToPlayer();
        BotModel.UpdateBotState(BotState.Idle);
    }
}
using UnityEngine;

public class GalaxyController : MonoBehaviour
{
    public GalaxyModel GalaxyModel;
    public PlayerStateModel PlayerStateModel;
    
    public void ZoomedIn()
    {
        GalaxyModel.FinishZoomingIn();
    }

    public void ZoomedOut()
    {
        GalaxyModel.FinishZoomingOut();
    }

    public void UnCoupleStar()
    {
        GalaxyModel.UnCoupleStar();
    }
    
    public void StartGame(Transform location)
    {
        GalaxyModel.OnZoomInComplete += SwitchGameMode;
        GalaxyModel.StartZooming(location.position);
        ZoomInGalaxy(location.transform, GalaxyButtonType.Play);
    }

    void SwitchGameMode()
    {
        GalaxyModel.StartGame();
        GalaxyModel.OnZoomInComplete -= SwitchGameMode;
        GalaxyModel.RenderingManager.enabled = true;
        PlayerStateModel.UpdateState(PlayerState.Placing);
    }

    public void ZoomInGalaxy(Transform zoomLocation, GalaxyButtonType type)
    {
        GalaxyModel.SelectedGalaxyButtonType = type;
        GalaxyModel.StartZooming(zoomLocation.position);
    }

    public void ZoomOutGalaxy(Transform zoomLocation)
    {
        GalaxyModel.StartZoomingOut(zoomLocation.position);
    }
}

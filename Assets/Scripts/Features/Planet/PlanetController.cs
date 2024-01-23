using UnityEngine;

public class PlanetController: MonoBehaviour
{
    public PlanetModel PlanetModel;
    public PlayerStateModel StateModel;
    public FaderModel FaderModel;
    
    public void LandOnPlanet()
    {
        FaderModel.StartFading();
        FaderModel.OnFullyFaded += SwitchToSurface;
    }

    public void BackToSpace()
    {
        FaderModel.OnFullyFaded += SwitchToSpace;
        FaderModel.StartFading();
    }

    void SwitchToSpace()
    {
        FaderModel.OnFullyFaded -= BackToSpace;
        StateModel.UpdateState(PlayerState.Space);
        RenderSettings.fog = false;
        RenderSettings.skybox = PlanetModel.SpaceSkybox;
    }
    
    void SwitchToSurface()
    {
        FaderModel.OnFullyFaded -= SwitchToSurface;
        StateModel.UpdateState(PlayerState.Surface);
        RenderSettings.skybox = PlanetModel.CurrentPlanetInfo.Skybox;
        RenderSettings.fog = true;
    }
}

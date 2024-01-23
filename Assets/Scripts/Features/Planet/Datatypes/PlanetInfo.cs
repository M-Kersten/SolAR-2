using UnityEngine;

[CreateAssetMenu(fileName = "Planet", menuName = "Planet", order = 0)]
public class PlanetInfo : ScriptableObject
{
    public PlanetName Name;
    public PlanetSurfaceView PlanetSurface;
    public Material Skybox;
    public DialogItem PlanetWelcomeDialog;
}
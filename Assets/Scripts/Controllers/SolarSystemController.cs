using UnityEngine;

public class SolarSystemController: MonoBehaviour
{
    public SolarSystemModel SolarSystemModel;

    public void SpawnPlanets()
    {
        SolarSystemModel.OnSpawnSolarSystem();
    }
}
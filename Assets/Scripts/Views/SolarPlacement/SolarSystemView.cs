using System;
using UnityEngine;

public class SolarSystemView: MonoBehaviour
{
    public GameObject Planets;
    public GameObject Sun;
    public GameObject Cursor;

    private SolarSystemModel _solarSystemModel;
    private PlayerStateModel _playerStateModel;
    
    private void Start()
    {
        transform.localScale = Vector3.zero;
        Planets.SetActive(false);
        _solarSystemModel = FindObjectOfType<SolarSystemModel>();
        if (_solarSystemModel)
            _solarSystemModel.OnSolarSystemSpawned += SpawnPlanets;

        _playerStateModel = FindObjectOfType<PlayerStateModel>();
        if (_playerStateModel)
            _playerStateModel.OnStateChanged += CheckSetActive;
    }

    private void OnDestroy()
    {
        if (_solarSystemModel)
            _solarSystemModel.OnSolarSystemSpawned -= SpawnPlanets;

        if (_playerStateModel)
            _playerStateModel.OnStateChanged -= CheckSetActive;
    }

    void CheckSetActive(PlayerState state)
    {
        bool isActive = _playerStateModel.CurrentPlayerState == PlayerState.Space;
        Planets.SetActive(isActive);
        Sun.SetActive(isActive);
    }
    
    void SpawnPlanets()
    {
        Cursor.SetActive(false);
        Planets.SetActive(true);
    }
}
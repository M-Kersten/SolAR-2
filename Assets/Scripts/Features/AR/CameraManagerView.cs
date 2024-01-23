using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManagerView : MonoBehaviour
{
    public Camera ArCamera;
    public Camera SkyboxCamera;
    
    private GalaxyModel _galaxyModel;
    private PlacementModel _placementModel;
    private PlayerStateModel _playerStateModel;
    
    private void Start()
    {
        _playerStateModel = FindObjectOfType<PlayerStateModel>();
        _playerStateModel.OnStateChanged += UpdateCameraViews;
        UpdateCameraViews(_playerStateModel.CurrentPlayerState);
    }

    private void OnDestroy()
    {
        _playerStateModel.OnStateChanged -= UpdateCameraViews;
    }

    void UpdateCameraViews(PlayerState state)
    {
        var isSkybox = state != PlayerState.Placing;
        if (isSkybox)
            ToSkybox();
        else
            ToAr();
    }

    void ToAr()
    {
        ArCamera.enabled = true;
        SkyboxCamera.enabled = false;
    }
    
    void ToSkybox()
    {
        ArCamera.enabled = false;
        SkyboxCamera.enabled = true;
    }
}

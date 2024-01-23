using System;

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StartGameButton: MonoBehaviour
{
    private GalaxyController _galaxyController;
    private Camera _arCamera;
    
    private void Start()
    {
        _arCamera = Camera.main.GetComponent<Camera>();
        _galaxyController = FindObjectOfType<GalaxyController>();
        if(_galaxyController)
            GetComponent<Button>().onClick.AddListener(() =>
            {
                _galaxyController.UnCoupleStar();
                _galaxyController.StartGame(transform);
            });
    }

    private void Update()
    {
        transform.rotation = _arCamera.transform.rotation;
    }
}

using System.Collections.Generic;
using UnityEngine.EventSystems;

using DG.Tweening;
using UnityEngine;

using Niantic.ARDK.Utilities;
using Niantic.ARDK.AR.HitTest;
using Niantic.ARDK.Utilities.Input.Legacy;


public class SolarPlacementController: MonoBehaviour
{
    public DialogController DialogController;
    public FaderModel FaderModel;
    public GalaxyModel GalaxyModel;
    public PlacementModel PlacementModel;
    public SolarSystemController SolarSystemController;
    public PlayerStateModel PlayerStateModel;

    private Vector3 objectDestination;
    private bool objectSpawned = false;
    private bool isPlacingObject;
    private SolarPlacementView _spawnedSolarSystem;


    private void Start()
    {
        SetPlacingActive(false);
        GalaxyModel.OnStartGame += () => SetPlacingActive(true);
        PlacementModel.OnSolarSystemPlaced += InitSolarSystem;
    }

    private void OnDestroy()
    {
        PlacementModel.OnSolarSystemPlaced -= InitSolarSystem;
    }

    void InitSolarSystem()
    {
        FaderModel.StartFading();
    }

    public void PlaceSolar()
    {
        PlacementModel.PlaceSolarSystem();
        FaderModel.OnFullyFaded -= PlaceSolar;
        SolarSystemController.SpawnPlanets();
        DialogController.WellDone();
        PlacementModel.RenderingManager.enabled = false;
        PlayerStateModel.UpdateState(PlayerState.Space);
    }

    public void SetPlacingActive(bool placing)
    {
        isPlacingObject = placing;
        PlacementModel.SolarSystem.Cursor.SetActive(placing);
        if(placing)
        {
            if (_spawnedSolarSystem == null)
                _spawnedSolarSystem = Instantiate(PlacementModel.SolarSystem, objectDestination, Quaternion.identity);
            
            objectDestination = _spawnedSolarSystem.transform.position;
        }
    }

    void Update() 
    { 
        if (!isPlacingObject || PlatformAgnosticInput.touchCount <= 0) 
            return;

        if (IsPointerOverUIObject())
            return;

        var touch = PlatformAgnosticInput.GetTouch(0);
        if (!objectSpawned && touch.phase == TouchPhase.Began)
        { 
            PlacementModel.MovedSolarSystem();
            objectSpawned = true;
            PositionObject(touch);
            _spawnedSolarSystem.transform.DOScale(Vector3.one, 1).SetEase(Ease.InOutQuad);
            DialogController.ExplainPlacement();
        }
        else if (touch.phase == TouchPhase.Moved && (Input.touchCount == 1 || Application.isEditor))
        {
            PositionObject(touch);
        }
        CheckForMultiTouch();

        _spawnedSolarSystem.transform.position = Vector3.Slerp(_spawnedSolarSystem.transform.position, objectDestination, Time.deltaTime * 10);
    }

    private bool IsPointerOverUIObject() {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
    
    private void CheckForMultiTouch()
    {
        if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
        {
            var curDist = Input.GetTouch(0).position - Input.GetTouch(1).position;
            var prevDist = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition));
            float touchDelta = curDist.magnitude - prevDist.magnitude;

            var MinMaxScale = PlacementModel.MinMaxScale;
            _spawnedSolarSystem.transform.localScale += Vector3.one * touchDelta * .0015f;
            if (_spawnedSolarSystem.transform.localScale.x > MinMaxScale.y)
                _spawnedSolarSystem.transform.localScale = Vector3.one * MinMaxScale.y;
            else if(_spawnedSolarSystem.transform.localScale.x < MinMaxScale.x)
                _spawnedSolarSystem.transform.localScale = Vector3.one * MinMaxScale.x;
        }
    }

    private void PositionObject(Touch touch) 
    {
          var currentFrame = _spawnedSolarSystem.Session.CurrentFrame;
          if (PlacementModel.ArCamera == null || currentFrame == null) 
              return;

          var hitTestResults = currentFrame.HitTest(
              PlacementModel.ArCamera.pixelWidth, 
              PlacementModel.ArCamera.pixelHeight, 
              touch.position, 
              ARHitTestResultType.EstimatedHorizontalPlane
          );

          _spawnedSolarSystem.Cursor.SetActive(hitTestResults.Count > 0);
          var objectPosition = hitTestResults.Count <= 0 
              ? GetMidairPosition(touch) 
              : hitTestResults[0].WorldTransform.ToPosition();

          if (_spawnedSolarSystem.Cursor.activeInHierarchy)
              _spawnedSolarSystem.Cursor.transform.position = objectPosition;
          
          objectPosition.y = PlacementModel.ArCamera.transform.position.y - .25f;
                  
          var newPosition = objectPosition;
          var objectTransform = _spawnedSolarSystem.transform;
          objectDestination = newPosition;

          objectTransform.eulerAngles = new Vector3(
              objectTransform.eulerAngles.x,
              PlacementModel.ArCamera.transform.eulerAngles.y,
              0);
    }

    Vector3 GetMidairPosition(Touch touch)
    {
        var distance = PlacementModel.SpawnDistance;
        var screenHeightPercentage = touch.position.y / Screen.height * distance;
        var adjustedSpawnDistance = distance * .5f + screenHeightPercentage * .5f;
        return PlacementModel.ArCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, adjustedSpawnDistance));
    }
}

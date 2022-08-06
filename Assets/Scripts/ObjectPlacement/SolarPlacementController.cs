using DG.Tweening;

using UnityEngine;

using Niantic.ARDK.Utilities;
using Niantic.ARDK.AR.HitTest;
using Niantic.ARDK.Extensions;
using Niantic.ARDK.Templates;
using Niantic.ARDK.Utilities.Input.Legacy;

using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SolarPlacementController: MonoBehaviour
{
    public float SpawnDistance;
    public Vector2 MinMaxScale;
    public ObjectHolderController objectController;
    public Button PlaceButton;
    public GameObject PlacementCanvas;
    public ARPlaneManager PlaneManager;
    public Image FadeImage;
    public ARRenderingManager RenderingManager;
    public RetrobotController RetroBot;
    public GameObject Starfield;
    
    private Vector3 objectDestination;
    private bool objectSpawned = false;
    private bool isPlacingObject;

    private void Start()
    {
        PlaceButton.onClick.AddListener(() =>
        {
            SetPlacingActive(false);
            FadeImage.DOFade(1, .2f).OnComplete(() =>
            {
                RetroBot.Talk("Well done! Now we can explore!");
                RenderingManager.enabled = false;
                FadeImage.DOFade(0, .2f);
            });
        });
    }

    public void SetPlacingActive(bool placing)
    {
        isPlacingObject = placing;
        Starfield.SetActive(!placing);
        PlacementCanvas.SetActive(placing);
        PlaneManager.enabled = placing;
        objectController.Cursor.SetActive(placing);
        if(!placing)
            PlaneManager.ClearAllPlanes();
        else
        {
            objectDestination = objectController.ObjectHolder.transform.position;
            RetroBot.gameObject.SetActive(true);
        }
    }

    void Update() 
    { 
        if (!isPlacingObject || PlatformAgnosticInput.touchCount <= 0) 
            return;

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        var touch = PlatformAgnosticInput.GetTouch(0);
        if (!objectSpawned && touch.phase == TouchPhase.Began)
        { 
            objectSpawned = true;
            PositionObject(touch);
        }
        else if (touch.phase == TouchPhase.Moved && (Input.touchCount == 1 || Application.isEditor))
        {
            PositionObject(touch);
        }
        CheckForMultiTouch();

        objectController.ObjectHolder.transform.position = Vector3.Slerp(objectController.ObjectHolder.transform.position, objectDestination, Time.deltaTime * 10);
    }

    private void CheckForMultiTouch()
    {
        if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
        {
            var curDist = Input.GetTouch(0).position - Input.GetTouch(1).position;
            var prevDist = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition));
            float touchDelta = curDist.magnitude - prevDist.magnitude;
            
            objectController.ObjectHolder.transform.localScale += Vector3.one * touchDelta * .0015f;
            if (objectController.ObjectHolder.transform.localScale.x > MinMaxScale.y)
                objectController.ObjectHolder.transform.localScale = Vector3.one * MinMaxScale.y;
            else if(objectController.ObjectHolder.transform.localScale.x < MinMaxScale.x)
                objectController.ObjectHolder.transform.localScale = Vector3.one * MinMaxScale.x;
        }
    }

    private void PositionObject(Touch touch) 
    {
          var currentFrame = objectController.Session.CurrentFrame;
          if (objectController.Camera == null || currentFrame == null) 
              return;

          var hitTestResults = currentFrame.HitTest(
              objectController.Camera.pixelWidth, 
              objectController.Camera.pixelHeight, 
              touch.position, 
              ARHitTestResultType.EstimatedHorizontalPlane
          );

          objectController.Cursor.SetActive(hitTestResults.Count > 0);
          var objectPosition = hitTestResults.Count <= 0 
              ? GetMidairPosition(touch) 
              : hitTestResults[0].WorldTransform.ToPosition();

          if (objectController.Cursor.activeInHierarchy)
              objectController.Cursor.transform.position = objectPosition;
          
          objectPosition.y = objectController.Camera.transform.position.y;
                  
          var newPosition = objectPosition;
          var objectTransform = objectController.ObjectHolder.transform;
          objectDestination = newPosition;

          objectController.ObjectHolder.SetActive(true);
          objectTransform.eulerAngles = new Vector3(
              objectTransform.eulerAngles.x,
              objectController.Camera.transform.eulerAngles.y,
              0);
    }

    Vector3 GetMidairPosition(Touch touch)
    {
        var screenHeightPercentage = touch.position.y / Screen.height * SpawnDistance;
        var adjustedSpawnDistance = SpawnDistance * .5f + screenHeightPercentage * .5f;
        return objectController.Camera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, adjustedSpawnDistance));
    }
}

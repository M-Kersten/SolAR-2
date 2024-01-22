using UnityEngine;

using Niantic.ARDK.Utilities;
using Niantic.ARDK.AR.HitTest;
using Niantic.ARDK.Utilities.Input.Legacy;

namespace Niantic.ARDK.Templates 
{
    public class PlacementController : MonoBehaviour
    {
        [HideInInspector]
        public ObjectHolderController OHcontroller;
        private Vector3 position;

        private void Start()
        {
            position = OHcontroller.ObjectHolder.transform.position;
        }

        void Update() 
        {
            if (PlatformAgnosticInput.touchCount <= 0) return;

            var touch = PlatformAgnosticInput.GetTouch(0);
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved) 
            {
                TouchBegan(touch);
            }

            OHcontroller.ObjectHolder.transform.position = Vector3.Slerp(OHcontroller.ObjectHolder.transform.position, position, Time.deltaTime * 10);
        }

        private void TouchBegan(Touch touch) 
        {
            var currentFrame = OHcontroller.Session.CurrentFrame;
            if (currentFrame == null) return;

            if (OHcontroller.Camera == null) return;

            var hitTestResults = currentFrame.HitTest (
                OHcontroller.Camera.pixelWidth, 
                OHcontroller.Camera.pixelHeight, 
                touch.position, 
                ARHitTestResultType.EstimatedHorizontalPlane
            );

            if (hitTestResults.Count <= 0) return;

            var groundPosition = hitTestResults[0].WorldTransform.ToPosition();
            groundPosition.y = OHcontroller.Camera.transform.position.y;
            
            var newPosition = groundPosition;
            var objectTransform = OHcontroller.ObjectHolder.transform;
            position = newPosition;

            OHcontroller.ObjectHolder.SetActive(true);
            objectTransform.eulerAngles = new Vector3(objectTransform.eulerAngles.x, 
                                                      OHcontroller.Camera.transform.eulerAngles.y, 
                                                    0);
        }
    }
}

using DG.Tweening;
using UnityEngine;

namespace Views
{
    public class GalaxyStarView: MonoBehaviour
    {
        public Transform GalaxyParent;

        private GalaxyModel _galaxyModel;
        
        private void Awake()
        {
            _galaxyModel = FindObjectOfType<GalaxyModel>();
            _galaxyModel.OnZoomInStart += HandleOnZoomInStart;
        }

        private void OnDestroy()
        {
            _galaxyModel.OnZoomInStart -= HandleOnZoomInStart;
        }

        private void HandleOnZoomInStart(Vector3 scalePivot)
        {
            transform.parent = GalaxyParent;
            transform.DOScale(Vector3.one * .25f, 1).SetDelay(2).SetEase(Ease.InOutSine);
        }
        
    }
}

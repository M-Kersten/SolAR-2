using DG.Tweening;

using UnityEngine;

namespace Views
{
    public class GalaxyStarView: MonoBehaviour
    {
        private GalaxyModel _galaxyModel;

        public Transform GalaxyParent;
        
        private void Awake()
        {
            _galaxyModel = FindObjectOfType<GalaxyModel>();
            if (_galaxyModel)
            {
                _galaxyModel.OnZoomInStart += (value) =>
                {
                    transform.parent = GalaxyParent;
                    transform.DOScale(Vector3.one * .25f, 1).SetDelay(2).SetEase(Ease.InOutSine);
                };
            }
        }
    }
}

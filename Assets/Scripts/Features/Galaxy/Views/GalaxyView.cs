using System;
using DG.Tweening;
using UnityEngine;

public class GalaxyView: MonoBehaviour
{
    private GalaxyController _galaxyController;
    private GalaxyModel _galaxyModel;

    public GameObject Star;
    
    private void Awake()
    {
        _galaxyController = FindObjectOfType<GalaxyController>();
        _galaxyModel = FindObjectOfType<GalaxyModel>();

        if (_galaxyModel)
        {
            _galaxyModel.OnZoomInStart += ScaleInGalaxy;
            _galaxyModel.OnZoomOutStart += ScaleOutGalaxy;
            _galaxyModel.OnUncoupleStar += UncoupleSun;
        }
    }

    private void OnDestroy()
    {
        if (_galaxyModel)
        {
            _galaxyModel.OnZoomInStart -= ScaleInGalaxy;
            _galaxyModel.OnZoomOutStart -= ScaleOutGalaxy;
            _galaxyModel.OnUncoupleStar -= UncoupleSun;
        }
    }

    void UncoupleSun()
    {
        Star.transform.SetParent(transform);
        Star.transform.DOScale(Star.transform.localScale * 3, _galaxyModel.ZoomInDuration).SetEase(Ease.InOutSine);
    }

    void ScaleInGalaxy(Vector3 location)
    {
        DOVirtual.Float(transform.localScale.x,  transform.localScale.x * _galaxyModel.ScaleAmount, _galaxyModel.ZoomInDuration, (value) =>
            {
                gameObject.ScaleFromPivot(location, Vector3.one * value);
            })
            .SetEase(Ease.InOutQuart)
            .OnComplete(() =>
            {
                _galaxyController.ZoomedIn();
            });
    }
    
    void ScaleOutGalaxy(Vector3 location)
    {
        DOVirtual.Float(transform.localScale.x, transform.localScale.x / _galaxyModel.ScaleAmount, _galaxyModel.ZoomInDuration, (value) =>
            {
                gameObject.ScaleFromPivot(location, Vector3.one * value);
            })
            .SetEase(Ease.InOutQuart)
            .OnComplete(() =>
            {
                _galaxyController.ZoomedOut();
            });
    }
}

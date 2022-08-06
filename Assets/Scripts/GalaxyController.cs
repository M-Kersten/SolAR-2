using System;

using DG.Tweening;

using Niantic.ARDK.Extensions;

using UnityEngine;
using UnityEngine.UI;

public class GalaxyController : MonoBehaviour
{
    public Button SunButton;
    public GameObject Galaxy;
    public GameObject GalaxyIntro;
    public GameObject GalaxyCanvas;
    public ARRenderingManager RenderingManager;
    public Image FadeImage;
    public SolarPlacementController SolarController;
    public GameObject Star;
    
    void Start()
    {
        SunButton.onClick.AddListener(StartGame);
        SolarController.SetPlacingActive(false);
    }

    public void StartGame()
    {
        Star.transform.parent = GalaxyIntro.transform;
        Star.transform.DOScale(Vector3.one * .25f, 1).SetDelay(2).SetEase(Ease.InOutSine);
        ZoomInGalaxy(SunButton.transform, () =>
        {
            FadeImage.DOFade(1, 1).OnComplete(() =>
            {
                Galaxy.SetActive(false);
                GalaxyIntro.SetActive(false);
                RenderingManager.enabled = true;
                SolarController.SetPlacingActive(true);
                FadeImage.DOFade(0, .5f);
            });
        });
    }

    public void ZoomInGalaxy(Transform zoomLocation, Action onZoom = null)
    {
        GalaxyCanvas.gameObject.SetActive(false);
        ScaleGalaxy(zoomLocation, true, onZoom);
    }

    public void ZoomOutGalaxy(Transform zoomLocation)
    {
        ScaleGalaxy(zoomLocation, false, () =>
        {
            GalaxyCanvas.gameObject.SetActive(true);
        });
    }

    
    void ScaleGalaxy(Transform location, bool ZoomIn, Action OnZoom = null)
    {
        
        DOVirtual.Float(Galaxy.transform.localScale.x, ZoomIn ? Galaxy.transform.localScale.x * 15 : Galaxy.transform.localScale.x / 15, 3, (value) =>
        {
            ScaleFromPivot(Galaxy, location.position, Vector3.one * value);
        })
            .SetEase(Ease.InOutQuart)
            .OnComplete(() =>
            {
                Debug.Log("invoking on zoom");
                OnZoom?.Invoke();
            });
    }

    public void ScaleFromPivot(GameObject target, Vector3 pivot, Vector3 newScale)
    {
        Vector3 A = target.transform.localPosition;
        Vector3 B = pivot;
 
        Vector3 C = A - B; // diff from object pivot to desired pivot/origin
 
        float RS = newScale.x / target.transform.localScale.x; // relataive scale factor
 
        // calc final position post-scale
        Vector3 FP = B + C * RS;
 
        // finally, actually perform the scale/translation
        target.transform.localScale = newScale;
        target.transform.localPosition = FP;
    }
}

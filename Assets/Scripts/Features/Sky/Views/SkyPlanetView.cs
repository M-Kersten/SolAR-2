using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkyPlanetView : MonoBehaviour
{
    [SerializeField]
    private GameObject ui;
    [SerializeField]
    private Image image;
    [SerializeField]
    private Image smallImage;
    [SerializeField]
    private Image icon;
    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private LineRenderer lineRenderer;

    private SkyModel skyModel;
    private PlanetSO so; // ref assets
    private HorizonVO vo; // ref simulation data
    
    public void Init(SkyModel skyModel, PlanetSO so, HorizonVO vo)
    {
        this.skyModel = skyModel;
        this.so = so;
        this.vo = vo;

        if (this.skyModel && this.vo.hasResponse && this.so)
        {
            name = this.so.title;

            if (image && so.planetSprite) {
                image.sprite = this.so.planetSprite;
                image.rectTransform.sizeDelta = this.so.planetSprite.rect.size;
            }

            if (smallImage && so.smallPlanetSprite) {
                smallImage.sprite = this.so.smallPlanetSprite;
                smallImage.rectTransform.sizeDelta = this.so.smallPlanetSprite.rect.size;
            }

            if (icon && so.iconSprite) {
                icon.sprite = this.so.iconSprite;
                RectTransform iconPos = icon.transform.parent.GetComponent<RectTransform>();
                iconPos.anchoredPosition = new Vector2(-so.nameXOffset, iconPos.anchoredPosition.y);
            }

            if (title) {
                title.text = so.title;
                RectTransform titlePos = title.transform.parent.GetComponent<RectTransform>();
                titlePos.anchoredPosition = new Vector2(so.nameXOffset, titlePos.anchoredPosition.y);
            }

            if (lineRenderer)
            {
                Color newColor = Color.white - so.trajectoryColor;
                newColor.a = 1;
                lineRenderer.material.SetColor("_Color", newColor);
            }
                

            this.skyModel.UpdateSimulationIndexHandler += UpdateSimulationIndexHandler;
            this.skyModel.UpdateSimulationHandler += UpdateSimulationHandler;
        }
    }

    // per interval / minute
    private void UpdateSimulationIndexHandler()
    {
        //transform.rotation = Quaternion.identity;
        //transform.Rotate(-Vector3.right, horizonVO.records[skyModel.CurrentSimulationIndex].elevation, Space.World);
        //transform.Rotate(Vector3.up, horizonVO.records[skyModel.CurrentSimulationIndex].azimuth, Space.World);

        UpdateSimulation();
    }

    // per frame
    private void UpdateSimulationHandler()
    {
        UpdateSimulation();
    }

    private void UpdateSimulation()
    {
        //Quaternion direction = Quaternion.Euler(new Vector3(-vo.currentRecord.elevation, vo.currentRecord.azimuth, 0));
        //transform.localPosition = direction * new Vector3(0, 0, so.distance);

        transform.localPosition = vo.currentPosition;

        ui.transform.LookAt(skyModel.Cam.transform);
        ui.transform.Rotate(0, 180, 0, Space.Self);

        lineRenderer.positionCount = vo.trajectoryPositions.Length;
        lineRenderer.SetPositions(vo.trajectoryPositions);
        lineRenderer.Simplify(1);

    }

    public SkySO SO()
    {
        return so;
    }
}

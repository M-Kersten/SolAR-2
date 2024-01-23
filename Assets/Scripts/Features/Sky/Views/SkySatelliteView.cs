using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkySatelliteView : MonoBehaviour
{
    [SerializeField]
    private GameObject ui;
    [SerializeField]
    private Image image;
    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private LineRenderer lineRenderer;

    private SkyModel skyModel;
    public SatelliteSO so; // ref assets
    private HorizonVO vo; // ref simulation data

    public void Init(SkyModel skyModel, SatelliteSO so, HorizonVO vo)
    {
        this.skyModel = skyModel;

        this.so = so;
        this.vo = vo;

        if (this.skyModel && this.vo.hasResponse && this.so)
        {
            name = this.so.title;

            if (this.so.satelliteSprite)
                image.sprite = this.so.satelliteSprite;

            if (title)
                title.text = so.title;

            this.skyModel.UpdateSimulationIndexHandler += UpdateSimulationIndexHandler;
            this.skyModel.UpdateSimulationHandler += UpdateSimulationHandler;
        }
    }

    // per interval / minute
    private void UpdateSimulationIndexHandler()
    {
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

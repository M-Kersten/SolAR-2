using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SkyController : MonoBehaviour
{
    [SerializeField]
    private SkyModel skyModel;

    private void Awake()
    {
        if (skyModel.IsActive && !skyModel.IsInitialized)
        {
            // TODO Store horizon response as json locally
            // TODO Check if local json is available

            skyModel.Cam = Camera.main;

            // Create Horizon VO's for request calls for planets and satellites
            InitRequestQueue();
            InitNextRequest();
        }
    }
    
    private void PauseUpdating()
    {
        skyModel.IsUpdating = false;
    }

    private void ResumeUpdating()
    {
        skyModel.IsUpdating = true;
    }

    // Init Horizon API requests qeueu for all planets and satellites
    private void InitRequestQueue()
    {
        skyModel.HorizonVOs = new List<HorizonVO>();

        var horizonVO = new HorizonVO
        {
            id = skyModel.SunSO.requestID,
            type = HorizonVO.TYPE_PLANET,
            requestStepSize = HorizonVO.STEP_SIZE_MINUTES,
            trajectoryLength = skyModel.PlanetTrajectoryLength,
            distance = skyModel.SunSO.distance
        };
        skyModel.HorizonVOs.Add(horizonVO);

        foreach (var planetSO in skyModel.PlanetSOs)
        {
            horizonVO = new HorizonVO
            {
                id = planetSO.requestID,
                type = HorizonVO.TYPE_PLANET,
                requestStepSize = HorizonVO.STEP_SIZE_MINUTES,
                trajectoryLength = skyModel.PlanetTrajectoryLength,
                distance = planetSO.distance
            };
            skyModel.HorizonVOs.Add(horizonVO);
        }

        foreach (var satelliteSO in skyModel.SatelliteSOs)
        {
            horizonVO = new HorizonVO
            {
                id = satelliteSO.requestID,
                type = HorizonVO.TYPE_SATELLITE,
                requestStepSize = HorizonVO.STEP_SIZE_MINUTES,
                trajectoryLength = skyModel.SatelliteTrajectoryLength,
                distance = satelliteSO.distance
            };
            skyModel.HorizonVOs.Add(horizonVO);
        }
    }

    // Await current request, response and parsing before calling next request, lazy loading of all planets and satellites to prevent requests being blocked by Horizon API
    public void InitNextRequest()
    {
        skyModel.CurrentHorizonVOIndex += 1;

        if (skyModel.CurrentHorizonVOIndex < skyModel.HorizonVOs.Count)
        {
            skyModel.CurrentHorizonVO = skyModel.HorizonVOs[skyModel.CurrentHorizonVOIndex];
            InitRequestURL();
        }
        else
        {
            RequestsComplete();
        }
    }

    public void InitRequestURL()
    {
        var requestURL = new StringBuilder(skyModel.HorizonBaseURL + "?");

        // Set general request parameters
        foreach (var parameter in skyModel.HorizonParameters)
            requestURL.Append(parameter + "&");

        // Set Horizon large body id
        requestURL.Append($"COMMAND='{skyModel.CurrentHorizonVO.id}'&");
        
        // Set location parameters
        var longitude = skyModel.TestLongitude.Replace(",", ".");
        var latitude = skyModel.TestLattitude.Replace(",", ".");
        requestURL.Append($"SITE_COORD='{longitude},{latitude},0'&");

        // Set time parameters
        var UTCOffset = TimeSpan.FromHours(DateTime.UtcNow.Hour - DateTime.Now.Hour);
        var startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).Add(UTCOffset);
        requestURL.Append($"START_TIME='{startTime:yyyy-MM-dd HH:mm:ss}'&");

        // Set day offset
        var dayOffset = TimeSpan.FromDays(1);
        var stopTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).Add(dayOffset).Add(UTCOffset);
        requestURL.Append($"STOP_TIME='{stopTime:yyyy-MM-dd HH:mm:ss}'&");

        // Set data step size parameter
        requestURL.Append($"STEP_SIZE='{skyModel.CurrentHorizonVO.requestStepSize}'");

        skyModel.CurrentHorizonVO.requestURL = requestURL.ToString();

        StartCoroutine(GetRequest(requestURL.ToString()));
    }

    IEnumerator GetRequest(string requestURL)
    {
        using var webRequest = UnityWebRequest.Get(requestURL);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError)
        {
            //InitNextRequest();
        }
        else
        {
            ParseResult(webRequest.downloadHandler.text);
        }
    }

    // Parse request response with static parsing logic
    private void ParseResult(string data)
    {
        var start = data.IndexOf(skyModel.HorizonParseStartChars);
        var end = data.IndexOf(skyModel.HorizonParseEndChars);
        data = data.Substring(start, end - start);

        var dataRows = data.Split('\n');

        skyModel.CurrentHorizonVO.records = new List<HorizonRecordVO>();

        foreach (var dataRow in dataRows)
        {
            // Split data in row values
            var dataRowValues = dataRow.Split(',');

            // Parse row values in correct format
            if (dataRowValues.Length >= 3)
            {
                var recordVO = new HorizonRecordVO
                {
                    // 1nd value is date 
                    date = dataRowValues[0],
                    // 4th value is azimuth
                    azimuth = float.Parse(dataRowValues[3]),
                    // 5th value is elevation
                    elevation = float.Parse(dataRowValues[4])
                };

                skyModel.CurrentHorizonVO.records.Add(recordVO);
            }
        }

        if(skyModel.CurrentHorizonVO.records.Count > 0)
            skyModel.CurrentHorizonVO.hasResponse = true;

        InitNextRequest();
    }

    private void RequestsComplete()
    {
        InitSun();
        InitPlanets();
        InitSatellites();

        // TODO Check if everything is initialized
        skyModel.IsInitialized = true;
        skyModel.IsUpdating = true;
        skyModel.CurrentSimulationIndex = DateTime.Now.Hour * 60 + DateTime.Now.Minute;

        InvokeRepeating(nameof(UpdateSimulationIndex), 0, skyModel.SimulationInterval);

        skyModel.InitializeComplete();
    }

    private void InitSun()
    {
        skyModel.SunVO = skyModel.GetHorizonVOByID(skyModel.SunSO.requestID);
        if(skyModel.SunVO != null && skyModel.SunVO.hasResponse)
        {
            var sun = Instantiate(skyModel.SunPrefab, skyModel.PlanetsHolder.transform);
            var planetView = sun.GetComponent<SkyPlanetView>();
            planetView.Init(skyModel, skyModel.SunSO, skyModel.SunVO);

            skyModel.PlanetGOs.Add(sun);
        }
    }

    private void InitPlanets()
    {
        foreach (var planetSO in skyModel.PlanetSOs)
        {
            var horizonVO = skyModel.GetHorizonVOByID(planetSO.requestID);
            if (horizonVO != null && horizonVO.hasResponse)
            {
                var planet = Instantiate(skyModel.PlanetsPrefab, skyModel.PlanetsHolder.transform);
                var planetView = planet.GetComponent<SkyPlanetView>();
                planetView.Init(skyModel, planetSO, horizonVO);

                skyModel.PlanetGOs.Add(planet);
            }
        }
    }

    private void InitSatellites()
    {
        foreach (var satelliteSO in skyModel.SatelliteSOs)
        {
            var horizonVO = skyModel.GetHorizonVOByID(satelliteSO.requestID);
            if (horizonVO != null && horizonVO.hasResponse)
            {
                var satellite = Instantiate(skyModel.SatellitesPrefab, skyModel.SatellitesHolder.transform);
                var satelliteView = satellite.GetComponent<SkySatelliteView>();
                satelliteView.Init(skyModel, satelliteSO, horizonVO);

                skyModel.SatelliteGOs.Add(satellite);
            }
        }
    }

    // Update simulation data every minute 
    private void UpdateSimulationIndex()
    {
        if (!skyModel.IsUpdating)
            return;
        
        skyModel.CurrentSimulationIndex = DateTime.Now.Hour * 60 + DateTime.Now.Minute;

        if (skyModel.CurrentSimulationIndex < skyModel.HorizonVOs[0].records.Count - 1)
        {
            foreach(var horizonVO in skyModel.HorizonVOs)
            {
                // Set azimuth / elevation data
                horizonVO.currentRecord = horizonVO.records[skyModel.CurrentSimulationIndex];
                horizonVO.nextRecord = horizonVO.records[skyModel.CurrentSimulationIndex + 1];
                horizonVO.difAzimuth = horizonVO.nextRecord.azimuth - horizonVO.currentRecord.azimuth;
                horizonVO.difElevation = horizonVO.nextRecord.elevation - horizonVO.currentRecord.elevation;

                // Set position
                var direction = Quaternion.Euler(new Vector3(-horizonVO.currentRecord.elevation, horizonVO.currentRecord.azimuth, 0));
                horizonVO.currentPosition = direction * new Vector3(0, 0, horizonVO.distance);

                // update trajectory positions
                horizonVO.trajectoryPositions = new Vector3[horizonVO.trajectoryLength];
                for (int i = 0; i < horizonVO.trajectoryLength; i++)
                {
                    // check if future trajectory is out of bounds
                    if (i < horizonVO.records.Count - 1)
                    {
                        var recordVO = horizonVO.records[skyModel.CurrentSimulationIndex + i - (horizonVO.trajectoryLength / 2)];
                        direction = Quaternion.Euler(new Vector3(-recordVO.elevation, recordVO.azimuth, 0));
                        var position = direction * new Vector3(0, 0, horizonVO.distance);

                        // take dif position because of local space linerenderer
                        horizonVO.trajectoryPositions[i] = position - horizonVO.currentPosition;
                    }
                }
            }
            skyModel.UpdateSimulationIndex();
        }
        else
        {
            skyModel.IsInitialized = false;
            CancelInvoke(nameof(UpdateSimulationIndex));
        }
    }

    private void Update()
    {
        if(skyModel.IsActive && skyModel.IsInitialized && skyModel.IsUpdating)
            UpdateSimulation();
    }

    // Smooth interpolate azimuth + elevation data between simulation intervals
    private void UpdateSimulation()
    {
        // calculate smooth animation based on elevation + azimuth + distance
        foreach (var horizonVO in skyModel.HorizonVOs)
        {
            var difAzimuth = horizonVO.difAzimuth / skyModel.SimulationInterval * Time.deltaTime;
            var difElevation = horizonVO.difElevation / skyModel.SimulationInterval * Time.deltaTime;

            horizonVO.currentRecord.azimuth += difAzimuth;
            horizonVO.currentRecord.elevation += difElevation;

            var direction = Quaternion.Euler(new Vector3(-horizonVO.currentRecord.elevation, horizonVO.currentRecord.azimuth, 0));
            horizonVO.currentPosition = direction * new Vector3(0, 0, horizonVO.distance);
        }

        skyModel.UpdateSimulation();
    }
}

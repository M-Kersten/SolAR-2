using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SkyController : MonoBehaviour
{
    [SerializeField]
    private SkyModel skyModel;
    
    private void PauseUpdating()
    {
        skyModel.IsUpdating = false;
    }

    private void ResumeUpdating()
    {
        skyModel.IsUpdating = true;
    }

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

    // Init Horizon API requests qeueu for all planets and satellites
    private void InitRequestQueue()
    {
        skyModel.HorizonVOs = new List<HorizonVO>();

        HorizonVO horizonVO;

        // init sun request
        horizonVO = new HorizonVO();
        horizonVO.id = skyModel.SunSO.requestID;
        horizonVO.type = HorizonVO.TYPE_PLANET;
        horizonVO.requestStepSize = HorizonVO.STEP_SIZE_MINUTES;
        horizonVO.trajectoryLength = skyModel.PlanetTrajectoryLength;
        horizonVO.distance = skyModel.SunSO.distance;
        skyModel.HorizonVOs.Add(horizonVO);

        // init planets request
        foreach (PlanetSO planetSO in skyModel.PlanetSOs)
        {
            horizonVO = new HorizonVO();
            horizonVO.id = planetSO.requestID;
            horizonVO.type = HorizonVO.TYPE_PLANET;
            horizonVO.requestStepSize = HorizonVO.STEP_SIZE_MINUTES;
            horizonVO.trajectoryLength = skyModel.PlanetTrajectoryLength;
            horizonVO.distance = planetSO.distance;
            skyModel.HorizonVOs.Add(horizonVO);
        }

        // init satellite request
        foreach (SatelliteSO satelliteSO in skyModel.SatelliteSOs)
        {
            horizonVO = new HorizonVO();
            horizonVO.id = satelliteSO.requestID;
            horizonVO.type = HorizonVO.TYPE_SATELLITE;
            horizonVO.requestStepSize = HorizonVO.STEP_SIZE_MINUTES;
            horizonVO.trajectoryLength = skyModel.SatelliteTrajectoryLength;
            horizonVO.distance = satelliteSO.distance;
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

    // Setup API get request call parameters
    public void InitRequestURL()
    {
        string requestURL = "";
        requestURL = skyModel.HorizonBaseURL + "?";

        // Set general request parameters
        int index = 0;
        foreach (string parameter in skyModel.HorizonParameters)
        {
            requestURL += parameter+"&";
            index++;
        }

        // Set Horizon large body id
        requestURL += "COMMAND='" + skyModel.CurrentHorizonVO.id + "'&";
        // Set location parameters
        string longitude = skyModel.TestLongitude.Replace(",", ".");
        string latitude = skyModel.TestLattitude.Replace(",", ".");
        requestURL += "SITE_COORD='" + longitude + "," + latitude + ",0'&";
        // Set time parameters
        TimeSpan UTCOffset = new TimeSpan(DateTime.UtcNow.Hour - DateTime.Now.Hour, 0, 0);
        //Studio05.Logger.Add("Horizon API - UTC offset: " + UTCOffset);
        DateTime startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        startTime = startTime.Add(UTCOffset);
        requestURL += "START_TIME='" + startTime.ToString("yyyy-MM-dd HH:mm:ss") + "'&";
        // Set day offset
        TimeSpan dayOffset = new TimeSpan(1, 0, 0, 0);
        DateTime stopTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        stopTime = stopTime.Add(dayOffset);
        stopTime = stopTime.Add(UTCOffset);
        requestURL += "STOP_TIME='" + stopTime.ToString("yyyy-MM-dd HH:mm:ss") + "'&";
        // Set data step size parameter
        requestURL += "STEP_SIZE='" + skyModel.CurrentHorizonVO.requestStepSize + "'";

        skyModel.CurrentHorizonVO.requestURL = requestURL;

        StartCoroutine(GetRequest(requestURL));
    }

    IEnumerator GetRequest(string requestURL)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(requestURL))
        {
            //Studio05.Logger.Add("Horizon API Get Request: " + requestURL);
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
    }

    // Parse request response with static parsing logic
    private void ParseResult(string data)
    {
        // Strip data between $$SOE and $$EOE characters
        int start = data.IndexOf(skyModel.HorizonParseStartChars);
        int end = data.IndexOf(skyModel.HorizonParseEndChars);
        data = data.Substring(start, end - start);
        //Studio05.Logger.Add("Data: " + data);

        // Split data in rows
        string[] dataRows = data.Split('\n');
        string[] dataRowValues;

        skyModel.CurrentHorizonVO.records = new List<HorizonRecordVO>();

        HorizonRecordVO recordVO;
        foreach (string dataRow in dataRows)
        {
            // Split data in row values
            dataRowValues = dataRow.Split(',');

            // Parse row values in correct format
            if (dataRowValues.Length >= 3)
            {
                recordVO = new HorizonRecordVO();

                // 1nd value is date 
                recordVO.date = dataRowValues[0];
                // 4th value is azimuth
                recordVO.azimuth = float.Parse(dataRowValues[3]);
                // 5th value is elevation
                recordVO.elevation = float.Parse(dataRowValues[4]);

                skyModel.CurrentHorizonVO.records.Add(recordVO);
            }
        }

        //Studio05.Logger.Add("azimuth: " + skyModel.CurrentHorizonVO.records[0].azimuth);

        if(skyModel.CurrentHorizonVO.records.Count > 0)
            skyModel.CurrentHorizonVO.hasResponse = true;

        //SaveJson(); // for testing purposes
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
        skyModel.CurrentSimulationIndex = (DateTime.Now.Hour * 60) + DateTime.Now.Minute;
        //skyModel.CurrentSimulationIndex = 708;

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
        HorizonVO horizonVO;
        foreach (var planetSO in skyModel.PlanetSOs)
        {
            horizonVO = skyModel.GetHorizonVOByID(planetSO.requestID);
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
        HorizonVO horizonVO;
        foreach (SatelliteSO satelliteSO in skyModel.SatelliteSOs)
        {
            horizonVO = skyModel.GetHorizonVOByID(satelliteSO.requestID);
            if (horizonVO != null && horizonVO.hasResponse)
            {
                GameObject satellite = Instantiate(skyModel.SatellitesPrefab, skyModel.SatellitesHolder.transform);
                SkySatelliteView satelliteView = satellite.GetComponent<SkySatelliteView>();
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
        
        //skyModel.CurrentSimulationIndex += 1;
        skyModel.CurrentSimulationIndex = (DateTime.Now.Hour * 60) + DateTime.Now.Minute;

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
                Quaternion direction = Quaternion.Euler(new Vector3(-horizonVO.currentRecord.elevation, horizonVO.currentRecord.azimuth, 0));
                horizonVO.currentPosition = direction * new Vector3(0, 0, horizonVO.distance);

                // update trajectory positions
                Vector3 position;
                HorizonRecordVO recordVO;
                horizonVO.trajectoryPositions = new Vector3[horizonVO.trajectoryLength];
                for (int i = 0; i < horizonVO.trajectoryLength; i++)
                {
                    // check if future trajectory is out of bounds
                    if (i < horizonVO.records.Count - 1)
                    {
                        recordVO = horizonVO.records[skyModel.CurrentSimulationIndex + i - (horizonVO.trajectoryLength / 2)];
                        direction = Quaternion.Euler(new Vector3(-recordVO.elevation, recordVO.azimuth, 0));
                        position = direction * new Vector3(0, 0, horizonVO.distance);

                        // take dif position because of local space linerenderer
                        horizonVO.trajectoryPositions[i] = position - horizonVO.currentPosition;
                    }
                }
            }

            //skyModel.CurrentSimulationTimeStamp = Time.time;
            skyModel.UpdateSimulationIndex();
        }
        else
        {
            skyModel.IsInitialized = false;
            CancelInvoke("UpdateSimulationIndex");
        }
    }

    private void Update()
    {
        if(skyModel.IsActive && skyModel.IsInitialized && skyModel.IsUpdating)
        {
            UpdateSimulation();
        }
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

    private void SaveJson()
    {
        // Parse Configuration VO to JSON
        string json = JsonUtility.ToJson(skyModel.CurrentHorizonVO);

        // Save JSON file locally
        string path = Application.dataPath + "/Resources/JSON/" + skyModel.CurrentHorizonVO.id + ".json";
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(json);
        writer.Close();
    }
}

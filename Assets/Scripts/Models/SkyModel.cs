using System.Collections.Generic;
using UnityEngine;

public class SkyModel : MonoBehaviour
{
    public delegate void EventHandler();
    public event EventHandler InitializeCompleteHandler;
    public event EventHandler UpdateSimulationIndexHandler;
    public event EventHandler UpdateSimulationHandler;

    public string TestLattitude;
    public string TestLongitude;
    
    [SerializeField]
    private bool isActive;
    
    // Base settings for request calls via Horizon API
    [SerializeField]
    private string horizonBaseURL;
    [SerializeField]
    private string[] horizonParameters;

    [SerializeField]
    private string horizonParseStartChars; 
    [SerializeField]
    private string horizonParseEndChars;

    // Specific settings for request calls + visuals
    [SerializeField]
    private PlanetSO sunSO;
    [SerializeField]
    private List<PlanetSO> planetSOs;
    [SerializeField]
    private List<SatelliteSO> satelliteSOs;

    // Prefabs + parents
    [SerializeField]
    private GameObject sunPrefab; // optionally sun can have a different prefab?
    [SerializeField]
    private GameObject planetsPrefab;
    [SerializeField]
    private GameObject planetsHolder;
    [SerializeField]
    private GameObject satellitesPrefab;
    [SerializeField]
    private GameObject satellitesHolder;

    [SerializeField]
    private int planetTrajectoryLength = 12; // amount of data points  
    [SerializeField]
    private int satelliteTrajectoryLength = 12; // amount of data points 

    [SerializeField]
    private float simulationInterval = 60; // in seconds, needs to be 60 since data step size is 1 minute. adjust for test purposes

    // Current request call index
    private int currentHorizonVOIndex = -1;
    // Current request call
    private HorizonVO currentHorizonVO;
    // List of all request calls
    private List<HorizonVO> horizonVOs;
    // Ref to sun VO
    private HorizonVO sunVO;

    private List<GameObject> planetGOs = new();
    private List<GameObject> satelliteGOs = new();

    private bool isInitialized;
    // Current simulation index based on minute of day
    private int currentSimulationIndex = -1;
    // Camera for lookat 
    private Camera cam;
    private bool isUpdating;

    internal void InitializeComplete()
    {
        InitializeCompleteHandler?.Invoke();
    }

    internal void UpdateSimulationIndex()
    {
        UpdateSimulationIndexHandler?.Invoke();
    }

    internal void UpdateSimulation()
    {
        UpdateSimulationHandler?.Invoke();
    }

    public HorizonVO GetHorizonVOByID(int id)
    {
        foreach (HorizonVO horizonVO in horizonVOs)
        {
            if (horizonVO.id == id)
            {
                return horizonVO;
            }
        }
        return null;
    }

    public HorizonVO GetSunHorizonVO()
    {
        foreach (HorizonVO horizonVO in horizonVOs)
        {
            if (horizonVO.id == sunSO.requestID)
            {
                return horizonVO;
            }
        }
        return null;
    }

    public bool IsActive { get => isActive; set => isActive = value; }
    public bool IsUpdating { get => isUpdating; set => isUpdating = value; }
    public bool IsInitialized { get => isInitialized; set => isInitialized = value; }
    public Camera Cam { get => cam; set => cam = value; }

    public string HorizonBaseURL { get => horizonBaseURL; set => horizonBaseURL = value; }
    public string[] HorizonParameters { get => horizonParameters; set => horizonParameters = value; }

    public string HorizonParseStartChars { get => horizonParseStartChars; set => horizonParseStartChars = value; }
    public string HorizonParseEndChars { get => horizonParseEndChars; set => horizonParseEndChars = value; }

    public PlanetSO SunSO { get => sunSO; set => sunSO = value; }
    public List<PlanetSO> PlanetSOs { get => planetSOs; set => planetSOs = value; }
    public List<SatelliteSO> SatelliteSOs { get => satelliteSOs; set => satelliteSOs = value; }

    public GameObject SunPrefab { get => sunPrefab; set => sunPrefab = value; }
    public GameObject PlanetsPrefab { get => planetsPrefab; set => planetsPrefab = value; }
    public GameObject PlanetsHolder { get => planetsHolder; set => planetsHolder = value; }
    public GameObject SatellitesPrefab { get => satellitesPrefab; set => satellitesPrefab = value; }
    public GameObject SatellitesHolder { get => satellitesHolder; set => satellitesHolder = value; }

    public int PlanetTrajectoryLength { get => planetTrajectoryLength; set => planetTrajectoryLength = value; }
    public int SatelliteTrajectoryLength { get => satelliteTrajectoryLength; set => satelliteTrajectoryLength = value; }
    public float SimulationInterval { get => simulationInterval; set => simulationInterval = value; }

    public List<GameObject> PlanetGOs { get => planetGOs; set => planetGOs = value; }
    public List<GameObject> SatelliteGOs { get => satelliteGOs; set => satelliteGOs = value; }

    public int CurrentHorizonVOIndex { get => currentHorizonVOIndex; set => currentHorizonVOIndex = value; }
    public HorizonVO CurrentHorizonVO { get => currentHorizonVO; set => currentHorizonVO = value; }
    public List<HorizonVO> HorizonVOs { get => horizonVOs; set => horizonVOs = value; }
    public HorizonVO SunVO { get => sunVO; set => sunVO = value; }

    public int CurrentSimulationIndex { get => currentSimulationIndex; set => currentSimulationIndex = value; }
}

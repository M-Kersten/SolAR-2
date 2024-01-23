using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HorizonVO
{
    public static string STEP_SIZE_DAYS = "1 DAYS";
    public static string STEP_SIZE_HOURS = "1 HOURS";
    public static string STEP_SIZE_MINUTES = "1 MINUTES";

    public static string TYPE_PLANET = "PLANET";
    public static string TYPE_SATELLITE = "SATELLITE";

    public string name;
    public int id;
    public string type;
    public float distance;

    public string requestURL;
    public string requestStepSize;
    public bool hasResponse = false;

    public List<HorizonRecordVO> records;
    public HorizonRecordVO currentRecord;
    public HorizonRecordVO nextRecord;
    public float difElevation;
    public float difAzimuth;

    public Vector3 currentPosition;

    public int trajectoryLength;
    public Vector3[] trajectoryPositions;

}

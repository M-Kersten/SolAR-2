using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HorizonRecordVO
{
    public string name;
    public string date;
    
    //public Vector3 position;

    // Azimuth is measured clockwise from north in degrees:
    // North(0) -> East(90) -> South(180) -> West(270)
    public float azimuth;

    // Elevation angle in degrees with respect to plane perpendicular to local zenith direction
    public float elevation;
}

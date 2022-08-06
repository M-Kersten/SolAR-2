using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetrobotThrusterControl : MonoBehaviour
{
    public GameObject thruster;
    public bool thrusterActivated = true;

    [Header("[Testing Only]")]
    public bool testing = false;
    
    void Start()
    {
        thruster.SetActive(thrusterActivated);
    }

    /// <summary>
    /// activate or deactivate the thruster. 
    /// </summary>
    /// <param name="activate">true or false, whether thruster is active</param>
    public void ActivateThruster(bool activate) {
        thruster.SetActive(activate);
    }


}

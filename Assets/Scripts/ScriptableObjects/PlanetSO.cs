using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Planet", menuName = "Niantic/Planet", order = 4)]
public class PlanetSO : SkySO
{
    [Header("Parameters")]
    public int requestID; // horizon api id
    //public string title;
    //public string description;
    public float distance = 1000; // used for z order
    public int nameXOffset = 0;

    [Header("Visuals")]
    public Sprite planetSprite;
    public Sprite smallPlanetSprite;
    public Sprite iconSprite;
    public Color trajectoryColor = Color.black;

    //public LowerThirdSO lowerThird;
}

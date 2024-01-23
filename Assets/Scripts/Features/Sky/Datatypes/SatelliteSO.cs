using UnityEngine;

[CreateAssetMenu(fileName = "Satellite", menuName = "Niantic/Satellite", order = 6)]
public class SatelliteSO : SkySO
{
    [Header("Parameters")]
    public int requestID; // horizon api id
    //public string title;
    public string description;
    public float distance = 750; // used for z order

    [Header("Visuals")]
    public Sprite satelliteSprite;

    //public LowerThirdSO lowerThird;
}
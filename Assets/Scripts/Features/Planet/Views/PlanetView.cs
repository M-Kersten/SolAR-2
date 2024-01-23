using System;

using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PlanetView : MonoBehaviour
{
    public PlanetName PlanetName;

    private BotController _botController;
    private DialogController _dialogController;
    private PlanetModel _planetModel;
    private PlanetController _planetController;

    private void Start()
    {
        _botController = FindObjectOfType<BotController>();
        _dialogController = FindObjectOfType<DialogController>();
        _planetModel = FindObjectOfType<PlanetModel>();
        _planetController = FindObjectOfType<PlanetController>();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag(_planetModel.VisitorColliderTag))
            VisitPlanet();
    }
    
    void VisitPlanet()
    {
        _dialogController.Visit(PlanetName.ToString());
        _botController.FlyToPlanet(transform.position);
        _planetModel.UpdatePlanetInfo(PlanetName);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(_planetModel.VisitorColliderTag))
            _botController.BackToPlayer();
    }

    private void OnMouseDown()
    {
        if (_planetModel.CurrentPlanetInfo.Name == PlanetName)
            _planetController.LandOnPlanet();
    }
}

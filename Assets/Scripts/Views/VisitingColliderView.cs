using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class VisitingColliderView : MonoBehaviour
{
    private Collider _collider;
    private SolarSystemModel _solarSystemModel;
    
    private void Start()
    {
        _collider = GetComponent<Collider>();
        _collider.enabled = false;

        _solarSystemModel = FindObjectOfType<SolarSystemModel>();
        _solarSystemModel.OnSolarSystemSpawned += DelayedEnableCollider;
    }

    void DelayedEnableCollider()
    {
        DOVirtual.DelayedCall(4, () => _collider.enabled = true);
    }
    
    
}

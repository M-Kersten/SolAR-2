using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class VisitingColliderView : MonoBehaviour
{
    private SphereCollider _collider;
    private SolarSystemModel _solarSystemModel;
    
    private void Start()
    {
        _collider = GetComponent<SphereCollider>();
        _collider.enabled = false;

        _solarSystemModel = FindObjectOfType<SolarSystemModel>();
        _solarSystemModel.OnSolarSystemSpawned += DelayedEnableCollider;
    }

    void DelayedEnableCollider()
    {
        DOVirtual.DelayedCall(4, () => _collider.enabled = true);
    }
    
    
}

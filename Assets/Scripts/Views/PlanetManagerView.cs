using UnityEngine;

public class PlanetManagerView : MonoBehaviour
{
    private PlanetModel _planetModel;
    private PlayerStateModel _stateModel;

    private PlanetSurfaceView ActiveSurface;
    private Camera _arCamera;
    
    void Start()
    {
        _arCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        _planetModel = FindObjectOfType<PlanetModel>();
        _stateModel = FindObjectOfType<PlayerStateModel>();
        _stateModel.OnStateChanged += UpdateSurface;
    }

    void UpdateSurface(PlayerState state)
    {
        if (state == PlayerState.Surface)
        {
            var planetInfo = _planetModel.CurrentPlanetInfo;
            
            ActiveSurface = Instantiate(planetInfo.PlanetSurface,
                Vector3.zero, 
                Quaternion.identity, transform);
        }
        else if (ActiveSurface != null)
        {
            Destroy(ActiveSurface.gameObject);
        }
    }
}

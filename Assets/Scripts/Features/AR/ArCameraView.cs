using Niantic.ARDK.AR;
using Niantic.ARDK.AR.ARSessionEventArgs;
using Niantic.ARDK.Utilities;
using UnityEngine;

public class ArCameraView: MonoBehaviour
{
    [SerializeField]
    private Camera _camera;

    private PlanetModel _planetModel;
    private PlayerStateModel _playerStateModel;
    private Vector3 _offset;
    
    public Camera Camera
    {
        get => _camera;
        set => _camera = value;
    }

    private IARSession _session;

    private void Start()
    {
        ARSessionFactory.SessionInitialized += _OnSessionInitialized;
        _planetModel = FindObjectOfType<PlanetModel>();
        _planetModel.OnSpawnedOnPlanet += CalculateOffset;
        _playerStateModel = FindObjectOfType<PlayerStateModel>();
        _playerStateModel.OnStateChanged += CheckForOffset;
    }

    void CheckForOffset(PlayerState state)
    {
        if (state != PlayerState.Surface)
            _offset = Vector3.zero;
    }
    
    void CalculateOffset(Transform position)
    {
        _offset = position.position - Camera.transform.position;
    }

    private void OnDestroy()
    {
        _planetModel.OnSpawnedOnPlanet -= CalculateOffset;
        _playerStateModel.OnStateChanged -= CheckForOffset;
        
        ARSessionFactory.SessionInitialized -= _OnSessionInitialized;

        var session = _session;
        if (session != null)
            session.FrameUpdated -= _FrameUpdated;
    }

    private void _OnSessionInitialized(AnyARSessionInitializedArgs args)
    {
        var oldSession = _session;
        if (oldSession != null)
            oldSession.FrameUpdated -= _FrameUpdated;

        var newSession = args.Session;
        _session = newSession;
        newSession.FrameUpdated += _FrameUpdated;
    }

    private void _FrameUpdated(FrameUpdatedArgs args)
    {
        var localCamera = Camera;
        if (localCamera == null)
            return;

        var session = _session;
        if (session == null)
            return;

        // Set the camera's position.
        var worldTransform = args.Frame.Camera.GetViewMatrix(Screen.orientation).inverse;
        localCamera.transform.position = worldTransform.ToPosition() + _offset;
        localCamera.transform.rotation = worldTransform.ToRotation();
    }
}
using Niantic.ARDK.AR;
using Niantic.ARDK.AR.ARSessionEventArgs;

using UnityEngine;
public class SolarPlacementView : MonoBehaviour
{
    public GameObject Cursor;
    
    
    private IARSession _session;
    public IARSession Session {
        get {return _session;}
    }

    void Start() 
    {
        ARSessionFactory.SessionInitialized += OnSessionInitialized;
    }

    private void OnSessionInitialized(AnyARSessionInitializedArgs args) 
    {
        ARSessionFactory.SessionInitialized -= OnSessionInitialized;
        _session = args.Session;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Niantic.ARDK.AR;
using Niantic.ARDK.AR.ARSessionEventArgs;

namespace Niantic.ARDK.Templates 
{
    public class ObjectHolderController : MonoBehaviour
    {
        public GameObject ObjectHolder;
        public GameObject Cursor;
        public Camera Camera;

        private IARSession _session;
        public IARSession Session {
            get {return _session;}
        }

        void Start() 
        {
            ARSessionFactory.SessionInitialized += OnSessionInitialized;
            ObjectHolder.SetActive(false);
        }

        private void OnSessionInitialized(AnyARSessionInitializedArgs args) 
        {
            ARSessionFactory.SessionInitialized -= OnSessionInitialized;
            _session = args.Session;
        }
    } 
}  

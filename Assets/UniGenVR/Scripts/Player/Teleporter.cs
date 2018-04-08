using UnityEngine;
using UniGenVR.Utils;
using UniPrep.Extensions;
using UnityEngine.Events;
using UniGenVR.Events;

namespace UniGenVR.Player {
    public class Teleporter : MonoBehaviour {
        [SerializeField] LayerMask blockingLayers;
        [SerializeField] LayerMask teleportableLayers;
        [SerializeField] float teleportDistance = 5;
        public Vector3HitUnityEvent OnTeleportUnityEvent = new Vector3HitUnityEvent();
        public Vector3HitUnityEvent OnGazeTeleportableUnityEvent = new Vector3HitUnityEvent();
        public UnityEvent OnGazeNonTeleportableUnityEvent;
      
        void OnEnable() {
            UGVRInput.OnDoubleClick += HandleOnDoubleClick;
        }

        void OnDisable() {
            UGVRInput.OnDoubleClick -= HandleOnDoubleClick;
        }

        void Update() {
            Raycast();
        }

        void Raycast() {
            RaycastHit? hit;
            if(PerformRaycast(out hit)) {
                if (OnGazeTeleportableUnityEvent != null)
                    OnGazeTeleportableUnityEvent.Invoke(((RaycastHit)hit).point);
            }
            else {
                if (OnGazeNonTeleportableUnityEvent != null)
                    OnGazeNonTeleportableUnityEvent.Invoke();
            }
        }

        bool PerformRaycast(out RaycastHit? outHit) {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit innerHit;

            if (Physics.Raycast(ray, out innerHit, teleportDistance, blockingLayers)) {
                int hitLayer = innerHit.collider.gameObject.layer;
                if (teleportableLayers.Contains(hitLayer)) {
                    outHit = innerHit;
                    return true;
                }
            }
            outHit = null;
            return false;
        }
        
        void HandleOnDoubleClick() {
            RaycastHit? hit;
            if (!PerformRaycast(out hit))
                return;

            if(OnTeleportUnityEvent != null) OnTeleportUnityEvent.Invoke(((RaycastHit)hit).point);
        }
    }
}

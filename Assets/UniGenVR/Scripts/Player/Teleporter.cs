using UnityEngine;
using UniPrep.Extensions;
using UnityEngine.Events;

namespace UniGenVR {
    public class Teleporter : MonoBehaviour {
        [SerializeField] LayerMask blockingLayers;
        [SerializeField] LayerMask teleportableLayers;
        [SerializeField] float teleportDistance = 5;
        public RaycastHitUnityEvent OnTeleport = new RaycastHitUnityEvent();
        public RaycastHitUnityEvent OnSeeTeleportable = new RaycastHitUnityEvent();
        public NullableRaycastHitUnityEvent OnSeeNonTeleportable = new NullableRaycastHitUnityEvent();

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
                if (OnSeeTeleportable != null)
                    OnSeeTeleportable.Invoke((RaycastHit)hit);
            }
            else {
                if (OnSeeNonTeleportable != null)
                    OnSeeNonTeleportable.Invoke(hit);
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

            if(OnTeleport != null) OnTeleport.Invoke((RaycastHit)hit);
        }
    }
}

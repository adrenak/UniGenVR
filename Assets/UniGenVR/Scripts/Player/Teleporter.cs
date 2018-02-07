using UnityEngine;
using UniPrep.Utils;
using UniGenVR.Utils;

namespace UniGenVR.Player {
    public class Teleporter : VRBehaviour {
        // always include UI layer
        public LayerMask teleportableLayers;
        public float teleportDistance = 5;
        public float fadeDuration = .33f;
        public GameObject markerPrefab;

        bool m_DidHitOnClick;
        Marker m_Marker;

        private void Start() {
            if (markerPrefab != null)
                m_Marker = Instantiate(markerPrefab).GetComponent<Marker>();
        }

        private void OnEnable() {
            VRInput.OnDown += HandleDown;
            VRInput.OnMaxHold += HandleMaxHold;
        }

        private void OnDisable() {
            VRInput.OnClick -= HandleDown;
            VRInput.OnMaxHold -= HandleMaxHold;
        }

        private void Update() {
            if (m_Marker) UpdateMarker();
        }

        void UpdateMarker() {
            RaycastHit? hit;
            var hitting = PerformRaycast(out hit);
            m_Marker.Set(hitting);
            reticle.Set(hitting);

            if (hitting){
                var sureHit = (RaycastHit)hit;
                m_Marker.transform.position = sureHit.point;
                m_Marker.transform.eulerAngles = new Vector3(-90, 0, 0);
            }
        }

        bool PerformRaycast(out RaycastHit? outputHit) {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit internalHit;

            if (Physics.Raycast(ray, out internalHit, teleportDistance, teleportableLayers)) {
                m_DidHitOnClick = true;
                outputHit = internalHit;

                // TODO: Make a second layer mask for this
                // Never consider UI layer
                if (internalHit.collider.gameObject.layer != 5)
                    return true;
                else
                    return false;
            }
            outputHit = null;
            return false;
        }

        private void HandleDown() {
            RaycastHit? hit;
            m_DidHitOnClick = PerformRaycast(out hit);
        }

        private void HandleMaxHold() {
            if (!m_DidHitOnClick) {
                m_DidHitOnClick = false;
                return;
            }

            RaycastHit? hit;
            if (!PerformRaycast(out hit))
                return;
            Vector3 hitPoint = ((RaycastHit)hit).point;

            Work fadeOutWork = new Work(cameraFade.BeginFadeOut(fadeDuration));
            fadeOutWork.Begin(() => {
                transform.position = new Vector3(
                    hitPoint.x,
                    hitPoint.y + playerEntity.Height,
                    hitPoint.z
                    );
                Work fadeInWork = new Work(cameraFade.BeginFadeIn(fadeDuration));
                fadeInWork.Begin();
            });
        }
    }
}

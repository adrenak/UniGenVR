using UnityEngine;
using UniPrep.Utils;
using UniGenVR.Utils;

namespace UniGenVR.Player {
    public class Teleporter : VRBehaviour {
        // always include UI layer
        public LayerMask teleportableLayers;
        public float teleportDistance = 5;
        public float fadeDuration = .33f;

        bool m_DidHitOnClick;

        private void OnEnable() {
            VRInput.OnDown += HandleDown;
            VRInput.OnMaxHold += HandleMaxHold;
        }

        private void OnDisable() {
            VRInput.OnClick -= HandleDown;
            VRInput.OnMaxHold -= HandleMaxHold;
        }

        bool CheckRaycast(out RaycastHit? pHit) {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, teleportDistance, teleportableLayers)) {
                m_DidHitOnClick = true;
                pHit = hit;

                // Never consider UI layer
                if (hit.collider.gameObject.layer != 5)
                    return true;
                else
                    return false;
            }
            pHit = null;
            return false;
        }

        private void HandleDown() {
            RaycastHit? hit;
            m_DidHitOnClick = CheckRaycast(out hit);
        }

        private void HandleMaxHold() {
            if (!m_DidHitOnClick) {
                m_DidHitOnClick = false;
                return;
            }

            RaycastHit? hit;
            if (!CheckRaycast(out hit))
                return;
            Vector3 hitPoint = ((RaycastHit)hit).point;

            Work fadeOutWork = new Work(cameraFade.BeginFadeOut(fadeDuration));
            fadeOutWork.Begin(() => {
                transform.position = new Vector3(
                    hitPoint.x,
                    hitPoint.y + playerEntity.height,
                    hitPoint.z
                    );
                Work fadeInWork = new Work(cameraFade.BeginFadeIn(fadeDuration));
                fadeInWork.Begin();
            });
        }
    }
}

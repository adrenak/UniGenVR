using UnityEngine;
using UniGenVR.Utils;
using UniGenVR.Player;

namespace UniGenVR.Player {
    public class Teleporter : VRBehaviour {
        public LayerMask m_LayerMask;
        public float m_TeleportDistance = 5;
        bool didHitOnClick;

        private void OnEnable() {
            VRInput.OnDown += HandleDown;
            VRInput.OnMaxHold += HandleMaxHold;
        }

        private void OnDIsable() {
            VRInput.OnClick -= HandleDown;
            VRInput.OnMaxHold -= HandleMaxHold;
        }

        bool CheckRaycast(out RaycastHit? pHit) {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, m_TeleportDistance, m_LayerMask)) {
                didHitOnClick = true;
                pHit = hit;
                return true;
            }
            pHit = null;
            return false;
        }

        private void HandleDown() {
            RaycastHit? hit;
            didHitOnClick = CheckRaycast(out hit);
        }

        private void HandleMaxHold() {
            if (!didHitOnClick)
                return;

            RaycastHit? hit;
            didHitOnClick = CheckRaycast(out hit);
            Vector3 hitPoint = ((RaycastHit)hit).point;
            transform.position = new Vector3(
                hitPoint.x,
                hitPoint.y + playerEntity.height,
                hitPoint.z
                );
        }
    }
}

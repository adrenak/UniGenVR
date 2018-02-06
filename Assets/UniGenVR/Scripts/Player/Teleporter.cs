using UnityEngine;
using UniPrep.Utils;
using UniGenVR.Utils;
using System.Collections;

namespace UniGenVR.Player {
    public class Teleporter : VRBehaviour {
        public LayerMask layerMask;
        public float teleportDistance = 5;
        public float fadeDuration = .33f;

        bool m_DidHitOnClick;

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

            if (Physics.Raycast(ray, out hit, teleportDistance, layerMask)) {
                m_DidHitOnClick = true;
                pHit = hit;
                return true;
            }
            pHit = null;
            return false;
        }

        private void HandleDown() {
            RaycastHit? hit;
            m_DidHitOnClick = CheckRaycast(out hit);
        }

        private void HandleMaxHold() {
            if (!m_DidHitOnClick)
                return;

            RaycastHit? hit;
            m_DidHitOnClick = CheckRaycast(out hit);
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

using System;
using UnityEngine;

namespace UniGenVR {
    // In order to interact with objects in the scene
    // this class casts a ray into the scene and if it finds
    // a VRInteractiveItem it exposes it for other classes to use.
    // This script should be generally be placed on the camera.
    public class EyeRaycaster : VRBehaviour {
        public event Action<RaycastHit> OnRaycasthit;                   // This event is called every frame that the user's gaze is over a collider.

        [SerializeField] private LayerMask m_ExclusionLayers;           // Layers to exclude from the raycast.
        [SerializeField] private bool m_ShowDebugRay;                   // Optionally show the debug ray.
        [SerializeField] private float m_DebugRayLength = 5f;           // Debug ray length.
        [SerializeField] private float m_DebugRayDuration = 1f;         // How long the Debug ray will remain visible.
        [SerializeField] private float m_RayLength = 500f;              // How far into the scene the ray is cast.

        //private VRReticle reticle;                                      // The reticle, if applicable.
        private Interactable m_CurrentInteractible;                //The current interactive item
        private Interactable m_LastInteractible;                   //The last interactive item

        // Utility for other classes to get the current interactive item
        public Interactable CurrentInteractible {
            get { return m_CurrentInteractible; }
        }

        private void OnEnable() {
            VRInput.OnClick += HandleClick;
            VRInput.OnDoubleClick += HandleDoubleClick;
            VRInput.OnUp += HandleUp;
            VRInput.OnDown += HandleDown;
        }

        private void OnDisable() {
            VRInput.OnClick -= HandleClick;
            VRInput.OnDoubleClick -= HandleDoubleClick;
            VRInput.OnUp -= HandleUp;
            VRInput.OnDown -= HandleDown;
        }

        private void Update() {
            EyeRaycast();
        }

        private void EyeRaycast() {
            // Show the debug ray if required
            if (m_ShowDebugRay) {
                Debug.DrawRay(transform.position, transform.forward * m_DebugRayLength, Color.blue, m_DebugRayDuration);
            }

            // Create a ray that points forwards from the camera.
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            // Do the raycast forweards to see if we hit an interactive item
            if (Physics.Raycast(ray, out hit, m_RayLength, ~m_ExclusionLayers)) {
                Interactable interactable = hit.collider.GetComponent<Interactable>(); //attempt to get the VRInteractiveItem on the hit object
                m_CurrentInteractible = interactable;

                // If we hit an interactive item and it's not the same as the last interactive item, then call Over
                if (interactable && interactable != m_LastInteractible) {
                    interactable.Over();
                }

                // Deactive the last interactive item 
                if (interactable != m_LastInteractible)
                    DeactiveLastInteractible();

                m_LastInteractible = interactable;

                // Something was hit, set at the hit position.
                if (reticle)
                    reticle.SetPosition(hit);

                if (OnRaycasthit != null)
                    OnRaycasthit(hit);
            }
            else {
                // Nothing was hit, deactive the last interactive item.
                DeactiveLastInteractible();
                m_CurrentInteractible = null;

                // Position the reticle at default distance.
                if (reticle)
                    reticle.SetPosition();
            }
        }

        private void DeactiveLastInteractible() {
            if (m_LastInteractible == null)
                return;

            m_LastInteractible.Out();
            m_LastInteractible = null;
        }

        private void HandleUp() {
            if (m_CurrentInteractible != null)
                m_CurrentInteractible.Up();
        }

        private void HandleDown() {
            if (m_CurrentInteractible != null)
                m_CurrentInteractible.Down();
        }

        private void HandleClick() {
            if (m_CurrentInteractible != null)
                m_CurrentInteractible.Click();
        }

        private void HandleDoubleClick() {
            if (m_CurrentInteractible != null)
                m_CurrentInteractible.DoubleClick();

        }
    }
}
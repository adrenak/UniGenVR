using System;
using UnityEngine;
using UnityEngine.UI;
using UniGenVR.Utils;
using UniGenVR.Component;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace UniGenVR.Player {
    // In order to interact with objects in the scene
    // this class casts a ray into the scene and if it finds
    // a Interactable it exposes it for other classes to use.
    // This script should be generally be placed on the camera.
    public class EyeRaycaster : VRBehaviour {
        public event Action<RaycastHit> OnRaycasthit;                   // This event is called every frame that the user's gaze is over a collider.

        [SerializeField] private Transform m_Camera;
        [SerializeField] private LayerMask m_ExcludedLayers;           // Layers to exclude from the raycast.
        [SerializeField] private bool m_ShowDebugRay;                   // Optionally show the debug ray.
        [SerializeField] private float m_RayLength = 500f;              // How far into the scene the ray is cast.

        Interactable m_CurrentInteractible;                //The current interactive item
        Interactable m_LastInteractible;                   //The last interactive item

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
            Raycast3D();
        }

        private void Raycast3D() {
            // Show the debug ray if required
            if (m_ShowDebugRay) {
                Debug.DrawRay(m_Camera.position, m_Camera.forward * m_RayLength, Color.blue, Time.deltaTime);
            }

            // Create a ray that points forwards from the camera.
            Ray ray = new Ray(m_Camera.position, m_Camera.forward);
            RaycastHit hit;

            // Do the raycast forweards to see if we hit an interactive item
            if (Physics.Raycast(ray, out hit, m_RayLength, ~m_ExcludedLayers)) {
                Interactable interactible = hit.collider.GetComponent<Interactable>(); //attempt to get the Interactable on the hit object
                m_CurrentInteractible = interactible;

                // If we hit an interactive item and it's not the same as the last interactive item, then call Over
                if (interactible && interactible != m_LastInteractible)
                    interactible.Over();

                // Deactive the last interactive item 
                if (interactible != m_LastInteractible)
                    DeactivateInteractable();

                m_LastInteractible = interactible;

                // Something was hit, set at the hit position.
                if (reticle)
                    reticle.SetPosition(hit);

                if (OnRaycasthit != null)
                    OnRaycasthit(hit);
            }
            else {
                // Nothing was hit, deactive the last interactive item.
                DeactivateInteractable();
                m_CurrentInteractible = null;

                // Position the reticle at default distance.
                if (reticle)
                    reticle.SetPosition();
            }
        }

        private void DeactivateInteractable() {
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
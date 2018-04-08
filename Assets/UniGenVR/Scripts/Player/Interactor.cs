using System;
using UnityEngine;
using UnityEngine.Events;
using UniGenVR.Utils;
using UniGenVR.Component;
using UniGenVR.Events;

namespace UniGenVR.Player {
    // In order to interact with objects in the scene
    // this class casts a ray into the scene and if it finds
    // a Interactable it exposes it for other classes to use.
    // This script should be generally be placed on the camera.
    public class Interactor : MonoBehaviour {
        [SerializeField] private LayerMask m_ExcludedLayers;
        [SerializeField] private bool m_ShowDebugRay;
        [SerializeField] private float m_RayLength = 500f;

        public event Action<Interactable> OnInteractableEvent;
        public InteractableUnityEvent OnInteractableUnityEvent = new InteractableUnityEvent();

        public event Action OnNoInteractableEvent;
        public UnityEvent OnNoInteractableUnityEvent;

        RaycastHit m_CurrentHit;
        Interactable m_CurrentInteractable;
        Interactable m_LastInteractable = null;

        void OnEnable() {
            UGVRInput.OnDown += HandleDown;
            UGVRInput.OnUp += HandleUp;
            UGVRInput.OnDoubleClick += HandleDoubleClick;
            UGVRInput.OnClick += HandleClick;
        }

        void OnDisable() {
            UGVRInput.OnDown -= HandleDown;
            UGVRInput.OnUp -= HandleUp;
            UGVRInput.OnDoubleClick -= HandleDoubleClick;
            UGVRInput.OnClick -= HandleClick;
        }

        public RaycastHit GetCurrentHit() {
            return m_CurrentHit;
        }

        public Interactable GetCurrentInteractable() {
            return m_CurrentInteractable;
        }

        void Update() {
            Raycast();
        }

        void Raycast() {
            if (m_ShowDebugRay) 
                Debug.DrawRay(transform.position, transform.forward * m_RayLength, Color.blue, Time.deltaTime);

            Ray ray = new Ray(transform.position, transform.forward);

            if (Physics.Raycast(ray, out m_CurrentHit, m_RayLength, ~m_ExcludedLayers)) {
                m_CurrentInteractable = m_CurrentHit.collider.gameObject.GetComponent<Interactable>();

                if (m_CurrentInteractable == null) {
                    OnNoInteractableUnityEvent.Invoke();
                    DeactivateLastInteractable();
                    return;
                }

                if (OnInteractableEvent != null) OnInteractableEvent(m_CurrentInteractable);
                if (OnInteractableUnityEvent != null) OnInteractableUnityEvent.Invoke(m_CurrentInteractable);

                // If we hit an interactive item and it's not the same as the last interactive item, then call Over
                if (m_CurrentInteractable != m_LastInteractable) {
                    DeactivateLastInteractable();
                    m_CurrentInteractable.Over();
                    m_LastInteractable = m_CurrentInteractable;
                }
            }
            else {
                OnNoInteractableUnityEvent.Invoke();
                // Nothing was hit, deactive the last interactive item.
                DeactivateLastInteractable();
                m_CurrentInteractable = null;
            }
        }

        void DeactivateLastInteractable() {
            if (m_LastInteractable == null)
                return;

            m_LastInteractable.Out();
            m_LastInteractable = null;
        }

        void HandleUp() {
            if (m_CurrentInteractable != null)
                m_CurrentInteractable.Up();
        }

        void HandleDown() {
            if (m_CurrentInteractable != null)
                m_CurrentInteractable.Down();
        }

        void HandleClick() {
            if (m_CurrentInteractable != null)
                m_CurrentInteractable.Click();
        }

        void HandleDoubleClick() {
            if (m_CurrentInteractable != null)
                m_CurrentInteractable.DoubleClick();
        }
    }
}
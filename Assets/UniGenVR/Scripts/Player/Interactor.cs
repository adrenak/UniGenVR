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

        public event Action<Interactable> OnInteractableHitEvent;
        public InteractableUnityEvent OnInteractableHitUnityEvent = new InteractableUnityEvent();

        public event Action<RaycastHit> OnHitEvent;
        public RaycastHitUnityEvent OnHitUnityEvent = new RaycastHitUnityEvent();

        public event Action OnNoHitEvent;
        public UnityEvent OnNoHitUnityEvent;

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
            Ray ray = new Ray(transform.position, transform.forward);

            if (m_ShowDebugRay)
                Debug.DrawRay(transform.position, transform.forward * 100, Color.blue, Time.deltaTime);


            if (Physics.Raycast(ray, out m_CurrentHit, Mathf.Infinity, ~m_ExcludedLayers)) {
                // Send event
                if (OnHitEvent != null) OnHitEvent(m_CurrentHit);
                if (OnHitUnityEvent != null) OnHitUnityEvent.Invoke(m_CurrentHit);

                m_CurrentInteractable = m_CurrentHit.collider.gameObject.GetComponent<Interactable>();
                if (m_CurrentInteractable == null) {
                    TryDeactivateLastInteractable();
                    return;
                }
                var distance = Vector3.Distance(transform.position, m_CurrentInteractable.transform.position);
                if (distance > m_CurrentInteractable.range)
                    return;

                // Send event
                if (OnInteractableHitEvent != null) OnInteractableHitEvent(m_CurrentInteractable);
                if (OnInteractableHitUnityEvent != null) OnInteractableHitUnityEvent.Invoke(m_CurrentInteractable);

                // If we hit an interactive item and it's not the same as the last interactive item, then call Over
                if (m_CurrentInteractable != m_LastInteractable) {
                    TryDeactivateLastInteractable();
                    m_CurrentInteractable.Over();
                }
                m_LastInteractable = m_CurrentInteractable;
            }
            else {
                // Send event
                if (OnNoHitEvent != null) OnNoHitEvent();
                if (OnNoHitUnityEvent != null) OnNoHitUnityEvent.Invoke();

                TryDeactivateLastInteractable();
                m_CurrentInteractable = null;
            }
        }

        void TryDeactivateLastInteractable() {
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
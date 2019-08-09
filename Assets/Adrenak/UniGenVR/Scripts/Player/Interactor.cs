using System;
using UnityEngine;
using UnityEngine.Events;

namespace Adrenak.UniGenVR {
    // In order to interact with objects in the scene
    // this class casts a ray into the scene and if it finds
    // a Interactable it exposes it for other classes to use.
    // This script should be generally be placed on the camera.
    public class Interactor : MonoBehaviour {
        [SerializeField] private LayerMask m_ExcludedLayers;
        [SerializeField] private bool m_ShowDebugRay;

        public RaycastHitUnityEvent OnHittingNonInteractable = new RaycastHitUnityEvent();
        public InteractableUnityEvent OnHittingInteractable = new InteractableUnityEvent();
        public FloatUnityEvent OnGazingInteractable = new FloatUnityEvent();
        public RaycastHitUnityEvent OnHitting = new RaycastHitUnityEvent();
        public UnityEvent OnNotHitting;

		Ray ray;
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

		public Ray GetRay() {
			return ray;
		}

        void Raycast() {
            ray = new Ray(transform.position, transform.forward);

            if (m_ShowDebugRay)
                Debug.DrawRay(transform.position, transform.forward * 100, Color.blue, Time.deltaTime);


            if (Physics.Raycast(ray, out m_CurrentHit, Mathf.Infinity, ~m_ExcludedLayers)) {
                // Send event
                if (OnHitting != null) OnHitting.Invoke(m_CurrentHit);

                m_CurrentInteractable = m_CurrentHit.collider.gameObject.GetComponent<Interactable>();
                if (m_CurrentInteractable == null) {
                    TryDeactivateLastInteractable();
                    if (OnHittingNonInteractable != null) OnHittingNonInteractable.Invoke(m_CurrentHit);
                    return;
                }
                var distance = Vector3.Distance(transform.position, m_CurrentInteractable.transform.position);
                if (distance > m_CurrentInteractable.range)
                    return;


                // If we hit an interactive item and it's not the same as the last interactive item, then call Over
                if (m_CurrentInteractable != m_LastInteractable) {
                    TryDeactivateLastInteractable();
                    m_CurrentInteractable.Over();
                }
                m_LastInteractable = m_CurrentInteractable;

                // Send event
                if (OnHittingInteractable != null) OnHittingInteractable.Invoke(m_CurrentInteractable);

                if (m_CurrentInteractable.FillCrosshair) {
                    if (OnHittingInteractable != null) OnGazingInteractable.Invoke(m_CurrentInteractable.TimerNormalized);
                }
            }
            else {
                // Send event
                if (OnNotHitting != null) OnNotHitting.Invoke();

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
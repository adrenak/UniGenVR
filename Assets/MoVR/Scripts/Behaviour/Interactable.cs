using System;
using UnityEngine;
using UnityEngine.Events;

namespace UniGenVR {
    // This class should be added to any gameobject in the scene
    // that should react to input based on the user's gaze.
    // It contains events that can be subscribed to by classes that
    // need to know about input specifics to this gameobject.
    public class Interactable : MonoBehaviour {
        public float range = Mathf.Infinity;
        [SerializeField] bool m_FillCrosshair = true;
        [SerializeField] float m_GazeDuration = 2;
        float m_Timer = 0;

        // Called when the gaze moves over this object
        public event Action OnOverEvent;
        public UnityEvent OnOverUnityEvent;

        // Called when the gaze leaves this object
        public event Action OnOutEvent;
        public UnityEvent OnOutUnityEvent;

        // Called every frame that the interactable is being gazed at
        public event Action<float> OnBeingGazedEvent;
        public FloatUnityEvent OnBeingGazedUnityEvent = new FloatUnityEvent();

        // called when the gaze has been going on for the maximum gaze duration
        public event Action OnGazedEvent;
        public UnityEvent OnGazedUnityEvent;

        // Called when click input is detected whilst the gaze is over this object.
        public event Action OnClickEvent;
        public UnityEvent OnClickUnityEvent;

        // Called when double click input is detected whilst the gaze is over this object.
        public event Action OnDoubleClickEvent;
        public UnityEvent OnDoubleClickUnityEvent;

        // Called when Fire1 is released whilst the gaze is over this object.
        public event Action OnUpEvent;
        public UnityEvent OnUpUnityEvent;

        // Called when Fire1 is pressed whilst the gaze is over this object.
        public event Action OnDownEvent;
        public UnityEvent OnDownUnityEvent;

        protected bool m_IsOver;

        public bool IsOver {
            get { return m_IsOver; }
        }

        public bool FillCrosshair {
            get { return m_FillCrosshair; }
        }

        public float GazeDuration {
            get { return m_GazeDuration; }
        }

        public float Timer {
            get { return m_Timer; }
        }

        public float TimerNormalized {
            get { return Timer / GazeDuration; }
        }

        private void Update() {
            if (m_IsOver) {
                m_Timer += Time.deltaTime;
                var normGazeDuration = m_Timer / m_GazeDuration;
                normGazeDuration = Mathf.Clamp01(normGazeDuration);
                if (OnBeingGazedEvent != null) OnBeingGazedEvent(normGazeDuration);
                if (OnBeingGazedUnityEvent != null) OnBeingGazedUnityEvent.Invoke(normGazeDuration);

                if (m_Timer > m_GazeDuration) {
                    m_IsOver = false;
                    m_Timer = 0;
                    TryInvoke(OnGazedEvent, OnGazedUnityEvent);
                }
            }
            else
                m_Timer = 0;
        }

        private void OnDrawGizmosSelected() {
            if (range == Mathf.Infinity)
                return;
            Gizmos.color = new Color(1, 0, 0, .1F);
            Gizmos.DrawSphere(transform.position, range);
        }

        public void Over() {
            m_IsOver = true;
            TryInvoke(OnOverEvent, OnOverUnityEvent);
        }

        public void Out() {
            m_IsOver = false;
            TryInvoke(OnOutEvent, OnOutUnityEvent);
        }
        
        public void Click() {
            TryInvoke(OnClickEvent, OnClickUnityEvent);
        }

        public void DoubleClick() {
            TryInvoke(OnDoubleClickEvent, OnDoubleClickUnityEvent);
        }

        public void Up() {
            TryInvoke(OnUpEvent, OnUpUnityEvent);
        }

        public void Down() {
            TryInvoke(OnDownEvent, OnDownUnityEvent);
        }

        void TryInvoke(Action a, UnityEvent e) {
            if (a != null) a();
            if (e != null) e.Invoke();
        }
    }
}
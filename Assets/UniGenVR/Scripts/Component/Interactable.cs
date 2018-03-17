using System;
using UnityEngine.Events;

namespace UniGenVR.Component {
    // This class should be added to any gameobject in the scene
    // that should react to input based on the user's gaze.
    // It contains events that can be subscribed to by classes that
    // need to know about input specifics to this gameobject.
    public class Interactable : VRBehaviour {
        public event Action OnOver;             // Called when the gaze moves over this object
        public event Action OnOut;              // Called when the gaze leaves this object
        public event Action OnClick;            // Called when click input is detected whilst the gaze is over this object.
        public event Action OnDoubleClick;      // Called when double click input is detected whilst the gaze is over this object.
        public event Action OnUp;               // Called when Fire1 is released whilst the gaze is over this object.
        public event Action OnDown;             // Called when Fire1 is pressed whilst the gaze is over this object.

        public UnityEvent OnOverUnityEvent;
        public UnityEvent OnOutUnityEvent;
        public UnityEvent OnClickUnityEvent;
        public UnityEvent OnDoubleClickUnityEvent;
        public UnityEvent OnUpUnityEvent;
        public UnityEvent OnDownUnityEvent;

        protected bool m_IsOver;


        public bool IsOver {
            get { return m_IsOver; }              // Is the gaze currently over this object?
        }


        // The below functions are called by the VREyeRaycaster when the appropriate input is detected.
        // They in turn call the appropriate events should they have subscribers.
        public void Over() {
            m_IsOver = true;

            if (OnOver != null)
                OnOver();
            if (OnOverUnityEvent != null)
                OnOverUnityEvent.Invoke();
        }


        public void Out() {
            m_IsOver = false;

            if (OnOut != null)
                OnOut();
            if (OnOutUnityEvent != null)
                OnOutUnityEvent.Invoke();
        }


        public void Click() {
            if (OnClick != null)
                OnClick();
            if (OnClickUnityEvent != null)
                OnClickUnityEvent.Invoke();
        }


        public void DoubleClick() {
            if (OnDoubleClick != null)
                OnDoubleClick();
            if (OnDoubleClickUnityEvent != null)
                OnDoubleClickUnityEvent.Invoke();
        }


        public void Up() {
            if (OnUp != null)
                OnUp();
            if (OnUpUnityEvent != null)
                OnUpUnityEvent.Invoke();
        }


        public void Down() {
            if (OnDown != null)
                OnDown();
            if (OnDownUnityEvent != null)
                OnDownUnityEvent.Invoke();
        }
    }
}
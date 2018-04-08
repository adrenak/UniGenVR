using UnityEngine;
using UnityEngine.Events;
using UniGenVR.Component;

namespace UniGenVR.Events {
    [System.Serializable]
    public class InteractableUnityEvent : UnityEvent<Interactable> { }

    [System.Serializable]
    public class Vector3HitUnityEvent : UnityEvent<Vector3> { }
}
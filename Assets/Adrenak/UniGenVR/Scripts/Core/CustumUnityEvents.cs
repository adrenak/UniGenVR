using UnityEngine;
using UnityEngine.Events;

namespace Adrenak.UniGenVR {
    [System.Serializable]
    public class InteractableUnityEvent : UnityEvent<Interactable> { }

    [System.Serializable]
    public class InteractableFloatUnityEvent : UnityEvent<Interactable, float> { }

    [System.Serializable]
    public class Vector3UnityEvent : UnityEvent<Vector3> { }

    [System.Serializable]
    public class RaycastHitUnityEvent : UnityEvent<RaycastHit> { }

    [System.Serializable]
    public class NullableRaycastHitUnityEvent : UnityEvent<RaycastHit?> { }

    [System.Serializable]
    public class FloatUnityEvent : UnityEvent<float> { }
}
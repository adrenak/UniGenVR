using System;
using UnityEngine;
using UniGenVR.Events;

namespace UniGenVR.Component {
    [RequireComponent(typeof(Interactable))]
    public class Waypoint : MonoBehaviour {
        public event Action<Vector3> OnInteractedEvent;
        public Vector3HitUnityEvent OnInteractedUnityEvent = new Vector3HitUnityEvent();

        // Gets the point directly below the waypoint
        public Vector3? GetGroundPoint() {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit))
                return hit.point;
            else
                return null;
        }

        public void Interact() {
            var groundPoint = GetGroundPoint();
            if(groundPoint != null)
                OnInteractedUnityEvent.Invoke((Vector3)groundPoint);
        }
    }
}

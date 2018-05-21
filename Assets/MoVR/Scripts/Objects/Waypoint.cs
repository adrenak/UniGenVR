using System;
using UnityEngine;

namespace UniGenVR {
    [RequireComponent(typeof(Interactable))]
    public class Waypoint : MonoBehaviour {
        public event Action<Vector3> OnInteractedEvent;
        public Vector3UnityEvent OnInteractedUnityEvent = new Vector3UnityEvent();

        // Gets the point directly below the waypoint
        public Vector3? GetGroundPoint() {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit))
                return hit.point;
            else
                return null;
        }

        public void Trigger() {
            var groundPoint = GetGroundPoint();
            if(groundPoint != null)
                OnInteractedUnityEvent.Invoke((Vector3)groundPoint);
        }
    }
}

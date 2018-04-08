using UnityEngine;
using UniGenVR.Events;

namespace UniGenVR.Component {
    [RequireComponent(typeof(Interactable))]
    public class Waypoint : MonoBehaviour {
        [SerializeField]
        Vector3HitUnityEvent OnGazedUnityEvent = new Vector3HitUnityEvent();

        // Gets the point directly below the waypoint
        public Vector3? GetGroundPoint() {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit))
                return hit.point;
            else
                return null;
        }

        public void OnGazed() {
            OnGazedUnityEvent.Invoke((Vector3)GetGroundPoint());
        }
    }
}

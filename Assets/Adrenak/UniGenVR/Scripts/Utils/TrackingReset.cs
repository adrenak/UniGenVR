using UnityEngine;
using UnityEngine.VR;

namespace Adrenak.UniGenVR {
    // This class simply insures the head tracking behaves correctly when the application is paused.
    public class TrackingReset : MonoBehaviour {
        private void OnApplicationPause(bool pauseStatus) {
            InputTracking.Recenter();
        }
    }
}
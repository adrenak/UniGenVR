using UnityEngine;

namespace Adrenak.UniGenVR {
    public class PlayerHeadSimulator : MonoBehaviour {
        public float rate;

        void Update() {
    #if UNITY_EDITOR || UNITY_STANDALONE
            if (Input.GetKey(KeyCode.D))
                transform.Rotate(Vector3.up, Time.deltaTime * rate, Space.World);
            if (Input.GetKey(KeyCode.A))
                transform.Rotate(Vector3.up, -Time.deltaTime * rate, Space.World);
            if (Input.GetKey(KeyCode.W))
                transform.Rotate(Vector3.right, -Time.deltaTime * rate, Space.Self);
            if (Input.GetKey(KeyCode.S))
                transform.Rotate(Vector3.right, Time.deltaTime * rate, Space.Self);

            if (Input.GetKey(KeyCode.E))
                transform.Rotate(Vector3.forward, -Time.deltaTime * rate, Space.Self);
            if (Input.GetKey(KeyCode.Q))
                transform.Rotate(Vector3.forward, Time.deltaTime * rate, Space.Self);
            if (!Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.Q)){
                transform.localEulerAngles = new Vector3(
                    transform.localEulerAngles.x,
                    transform.localEulerAngles.y,
                    0
                );
            }
    #endif
        }
    }
}
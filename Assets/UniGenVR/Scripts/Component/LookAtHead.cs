using UniGenVR.Player;
using UnityEngine;

namespace UniGenVR.Component {
    public class LookAtHead : MonoBehaviour {
        Transform m_Target;

        private void Start() {
            m_Target = GameObject.FindObjectOfType<PlayerHead>().transform;
        }

        private void Update() {
            if(m_Target != null)
                transform.LookAt(m_Target);
        }
    }
}

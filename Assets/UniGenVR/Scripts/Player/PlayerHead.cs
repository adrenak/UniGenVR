using UnityEngine;

namespace UniGenVR {
    [ExecuteInEditMode]
    public class PlayerHead : MonoBehaviour {
        PlayerBody m_Body;

        void OnEnable() {
            if (m_Body == null) {
                m_Body = GameObject.FindObjectOfType<PlayerBody>();
                m_Body.headTransform = transform;
            }
        }

        void Update() {
            var bodyOrientation = m_Body.transform.eulerAngles;
            m_Body.transform.eulerAngles = new Vector3(
                bodyOrientation.x,
                transform.eulerAngles.y,
                bodyOrientation.z
            );
        }
    }
}

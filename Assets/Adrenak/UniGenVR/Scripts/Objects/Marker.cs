using UnityEngine;

namespace Adrenak.UniGenVR {
    public class Marker : MonoBehaviour {
        [SerializeField] float m_RotationSpeed;
        [SerializeField] float m_MaxScaleFactor;
        [SerializeField] float m_MinScaleFactor;
        [SerializeField] float m_ScaleSpeed;
        [SerializeField] Vector3 m_EulerAngles;

        Renderer m_Renderer;

        public void Set(bool flag) {
            m_Renderer.enabled = flag;
        }

        public void Place(Vector3 position) {
            transform.position = position;
            transform.localEulerAngles = m_EulerAngles;
        }

        void Awake() {
            m_Renderer = GetComponent<Renderer>();
        }

        void Update() {
            Rotate();
            Scale();
        }

        void Rotate() {
            transform.Rotate(new Vector3(0, 0, m_RotationSpeed));
        }

        void Scale() {
            transform.localScale = new Vector3(
                transform.localScale.x,
                transform.localScale.y,
                (m_MinScaleFactor + Mathf.PingPong(Time.time * m_ScaleSpeed, m_MaxScaleFactor - m_MinScaleFactor))
            );
        }
    }
}

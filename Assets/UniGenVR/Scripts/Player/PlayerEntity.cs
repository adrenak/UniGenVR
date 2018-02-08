using UnityEngine;

namespace UniGenVR.Player {
    public class PlayerEntity : VRBehaviour {
        [SerializeField] LayerMask m_WalkableLayer;
        [SerializeField] float m_Height = 1.7f;
        [SerializeField] float m_BlendSpeed = 10;

        public float Height { get { return m_Height; } }

        private void Update() {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, new Vector3(0, -1, 0), out hit, Mathf.Infinity, m_WalkableLayer)) {
                transform.position = new Vector3(
                    transform.position.x,
                    Mathf.Lerp(
                        transform.position.y, 
                        hit.point.y + m_Height,
                        Time.deltaTime * Time.timeScale * m_BlendSpeed
                    ),
                    transform.position.z
                );
            }
        }
    }
}

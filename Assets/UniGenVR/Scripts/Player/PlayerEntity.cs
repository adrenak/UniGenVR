using UnityEngine;

namespace UniGenVR.Player {
    public class PlayerEntity : VRBehaviour {
        public LayerMask m_WalkableLayer;
        public float height;
        public float m_Smoothness = 1;

        private void Update() {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, new Vector3(0, -1, 0), out hit, Mathf.Infinity, m_WalkableLayer)) {
                transform.position = new Vector3(
                    transform.position.x,
                    Mathf.Lerp(
                        transform.position.y, 
                        hit.point.y + height,
                        Time.deltaTime * Time.timeScale * m_Smoothness
                    ),
                    transform.position.z
                );
            }
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UniGenVR {
    // The reticle is a small point at the centre of the screen.
    // It is used as a visual aid for aiming.
    public class Crosshair : MonoBehaviour {
        // The default distance away from the camera the reticle is placed.
        [SerializeField] float m_DefaultDistance = 5f;

        // Whether the reticle should be placed parallel to a surface.
        [SerializeField] bool m_UseNormal;

        // We need to affect the reticle's transform.
        [SerializeField] Transform m_ReticleTransform;

        // Reference to the image component that represents the reticle.
        Image m_Image;

        // Since the scale of the reticle changes, the original scale needs to be stored.
        Vector3 m_OriginalScale;

        // Used to store the original rotation of the reticle.
        Quaternion m_OriginalRotation;

        public bool UseNormal {
            get { return m_UseNormal; }
            set { m_UseNormal = value; }
        }

        public Transform ReticleTransform { get { return m_ReticleTransform; } }
        
        void Awake() {
            // Store the original scale and rotation.
            m_OriginalScale = m_ReticleTransform.localScale;
            m_OriginalRotation = m_ReticleTransform.localRotation;

            m_Image = m_ReticleTransform.GetComponent<Image>();
            HideReticle();
        }

        public void SetReticle(bool flag) {
            m_Image.enabled = flag;
        }

        public void HideReticle() {
            SetReticle(false);
        }

        public void ShowReticle() {
            SetReticle(true);
        }

        public void ResetPosition() {
            // Set the position of the reticle to the default distance in front of the camera.
            m_ReticleTransform.position = transform.position + transform.forward * m_DefaultDistance;

            // Set the scale based on the original and the distance from the camera.
            m_ReticleTransform.localScale = m_OriginalScale * m_DefaultDistance;

            // The rotation should just be the default.
            m_ReticleTransform.localRotation = m_OriginalRotation;
        }

        public void SetPosition(RaycastHit hit) {
            m_ReticleTransform.position = hit.point;
            m_ReticleTransform.localScale = m_OriginalScale * hit.distance;

            // If the reticle should use the normal of what has been hit...
            if (m_UseNormal)
                // Set it's rotation based on it's forward vector facing along the normal.
                m_ReticleTransform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
            else
                // However if it isn't using the normal then it's local rotation should be as it was originally.
                m_ReticleTransform.localRotation = m_OriginalRotation;
        }
    }
}
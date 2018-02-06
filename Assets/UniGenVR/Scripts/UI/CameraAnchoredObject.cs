using UnityEngine;
using UniPrep.Utils;

namespace UniGenVR.UI {
    // This class is used to move UI elements in ways that are
    // generally useful when using VR, specifically looking at
    // the camera and rotating so they're always in front of
    // the camera.
    public class CameraAnchoredObject : MonoBehaviour {
        [SerializeField] Transform m_CameraTransform;
        [SerializeField] float m_FollowSpeed = 2;

        float m_DistanceFromCamera;
        float m_OriginalFollowSpeed;

        /// <summary>
        /// Snaps the object into the target position and rotation in a single frame
        /// </summary>
        public void Snap() {
            var followSpeedTmp = m_FollowSpeed;
            m_FollowSpeed = Mathf.Infinity;

            Work.StartDelayed(Time.deltaTime, () => {
                m_FollowSpeed = followSpeedTmp;
            });
        }

        /// <summary>
        /// Sets the follow speed to an arbitrary value
        /// </summary>
        /// <param name="followSpeed"></param>
        public void SetFollowSpeed(float followSpeed) {
            m_FollowSpeed = followSpeed;
        }

        /// <summary>
        /// Resets the follow speed to the original vlaue
        /// </summary>
        public void ResetFollowSpeed() {
            m_FollowSpeed = m_OriginalFollowSpeed;
        }

        void Start() {
            m_DistanceFromCamera = Vector3.Distance(transform.position, m_CameraTransform.position);
            m_OriginalFollowSpeed = m_FollowSpeed;
        }

        void Update() {
            // Set it's rotation to point from the UI to the camera.
            transform.rotation = Quaternion.LookRotation(transform.position - m_CameraTransform.position);

            // Find the direction the camera is looking but on a flat plane.
            Vector3 targetDirection = Vector3.ProjectOnPlane(m_CameraTransform.forward, Vector3.up).normalized;

            // Calculate a target position from the camera in the direction at the same distance from the camera as it was at Start.
            Vector3 targetPosition = m_CameraTransform.position + targetDirection * m_DistanceFromCamera;

            // Set the target position  to be an interpolation of itself and the UI's position.
            targetPosition = Vector3.Lerp(transform.position, targetPosition, m_FollowSpeed * Time.deltaTime);

            // Since the UI is only following on the XZ plane, negate any y movement.
            targetPosition.y = transform.position.y;

            // Set the UI's position to the calculated target position.
            transform.position = targetPosition;
        }
    }
}
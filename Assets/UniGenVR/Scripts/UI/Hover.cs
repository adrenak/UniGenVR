using UnityEngine;
using UniPrep.Utils;

namespace UniGenVR.UI {
    public class Hover : MonoBehaviour {
        [SerializeField] Transform m_SubjectTransform;
        [SerializeField] float m_FollowSpeed = 2;

        [SerializeField] float m_DistanceFromSubject;
        [SerializeField] float m_HeightFromSubject;

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

        void Awake() {
            m_OriginalFollowSpeed = m_FollowSpeed;
        }

        void Update() {
            // Set it's rotation to point from the UI to the camera.
            transform.rotation = Quaternion.LookRotation(transform.position - m_SubjectTransform.position);

            // Find the direction the camera is looking but on a flat plane.
            Vector3 targetDirection = Vector3.ProjectOnPlane(m_SubjectTransform.forward, Vector3.up).normalized;

            // Calculate a target position from the camera in the direction at the same distance from the camera as it was at Start.
            Vector3 targetPosition = m_SubjectTransform.position + targetDirection * m_DistanceFromSubject;
            targetPosition.y = m_SubjectTransform.position.y + m_HeightFromSubject;

            // Set the target position  to be an interpolation of itself and the UI's position.
            targetPosition = Vector3.Lerp(transform.position, targetPosition, m_FollowSpeed * Time.deltaTime);

            // Set the position to the calculated target position.
            transform.position = targetPosition;
        }
    }
}
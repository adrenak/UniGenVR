using UnityEngine;
using UniPrep.Utils;

namespace UniGenVR {
    [ExecuteInEditMode]
    public class Hover : MonoBehaviour {
        [SerializeField] Transform m_SubjectTransform;

        [SerializeField] float m_Distance;

        [Range(-90, 90)]
        [SerializeField] float m_Pitch;

        [Range(-180, 190)]
        [SerializeField] float m_Yaw;

        [SerializeField] float m_FollowRate = 2;

        float m_OriginalFollowSpeed;

        /// <summary>
        /// Snaps the object into the target position and rotation in a single frame
        /// </summary>
        public void Snap() {
            var followSpeedTmp = m_FollowRate;
            m_FollowRate = Mathf.Infinity;

            Work.StartDelayed(Time.deltaTime, () => {
                m_FollowRate = followSpeedTmp;
            });
        }

        /// <summary>
        /// Sets the follow speed to an arbitrary value
        /// </summary>
        /// <param name="followSpeed"></param>
        public void SetFollowSpeed(float followSpeed) {
            m_FollowRate = followSpeed;
        }

        /// <summary>
        /// Resets the follow speed to the original vlaue
        /// </summary>
        public void ResetFollowSpeed() {
            m_FollowRate = m_OriginalFollowSpeed;
        }

        void Awake() {
            m_OriginalFollowSpeed = m_FollowRate;
        }

        void Update() {
            // Clamp pitch and yaw
            m_Pitch = Mathf.Clamp(m_Pitch, -90, 90);

            m_Yaw = Mathf.Clamp(m_Yaw, -180, 180);
            var adjustedYaw = m_SubjectTransform.localEulerAngles.y + m_Yaw;

            // Set it's rotation to point from the UI to the camera.
            transform.rotation = Quaternion.LookRotation(transform.position - m_SubjectTransform.position);

            Vector3 targetPosition = new Vector3(
                m_SubjectTransform.position.x + (m_Distance * Mathf.Cos(Mathf.Deg2Rad * m_Pitch) * Mathf.Sin(Mathf.Deg2Rad * adjustedYaw)),
                m_SubjectTransform.position.y + (m_Distance * Mathf.Sin(Mathf.Deg2Rad * m_Pitch)),
                m_SubjectTransform.position.z + (m_Distance * Mathf.Cos(Mathf.Deg2Rad * m_Pitch) * Mathf.Cos(Mathf.Deg2Rad * adjustedYaw))
            );

            // Set the target position to be an interpolation of itself and the UI's position.
            targetPosition = Vector3.Lerp(transform.position, targetPosition, m_FollowRate * Time.deltaTime);

            // Set the position to the calculated target position.
            transform.position = targetPosition;
        }
    }
}
using UnityEngine;
using UniPrep.Utils;
using UniGenVR.UI;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterController))]
public class PlayerRig : MonoBehaviour {
    public float height = 1.8F;
    public float radius = 0.5F;

    [SerializeField] float m_TeleportFadeDuration = .5F;
    [SerializeField] Transform m_CameraTransform;

    CapsuleCollider m_CapsuleCollider;
    NavMeshAgent m_NavMeshAgent;
    CharacterController m_CharacterController;

    public Fader cameraFader {
        get { return m_CameraTransform.GetComponent<Fader>(); }
    }

    // ================================================
    // MOVEMENT
    // ================================================
    public void MoveTo(Vector3 destination) {
        m_NavMeshAgent.destination = destination;
    }

    public void Teleport(Vector3 hitPoint) {
        Work fadeOutWork = new Work(cameraFader.BeginFadeOut(m_TeleportFadeDuration));
        fadeOutWork.Begin(() => {
            transform.position = new Vector3(
                hitPoint.x,
                hitPoint.y,
                hitPoint.z
            );
            Work fadeInWork = new Work(cameraFader.BeginFadeIn(m_TeleportFadeDuration));
            fadeInWork.Begin();
        });
    }

    void Start() {
        m_CapsuleCollider = GetComponent<CapsuleCollider>();
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_CharacterController = GetComponent<CharacterController>();
    }

    void Update() {
        UpdateFromDimensions();
    }

    void UpdateFromDimensions() {
        m_CapsuleCollider.radius = radius;
        m_CapsuleCollider.height = height;
        m_CapsuleCollider.center = new Vector3(0, height / 2, 0);

        m_NavMeshAgent.radius = radius;
        m_NavMeshAgent.height = height;

        m_CharacterController.height = height;
        m_CharacterController.radius = radius;
        m_CharacterController.center = new Vector3(0, height / 2, 0);
    }
}

﻿using UnityEngine;
using Adrenak.Unex;
using UnityEngine.AI;

namespace Adrenak.UniGenVR {
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(CharacterController))]
    public class PlayerBody : MonoBehaviour {
        public PlayerMovementMode defaultMovementMode;
        public float height = 1.8F;
        public float radius = 0.5F;
        public Transform headTransform;

        [SerializeField] bool m_FallbackToTeleport;
        [SerializeField] float m_TeleportFadeDuration = .1F;

        NavMeshAgent m_NavMeshAgent;
        CharacterController m_CharacterController;

        public Fader cameraFader {
            get { return headTransform.GetComponent<Fader>(); }
        }

        // ================================================
        // MOVEMENT
        // ================================================
        public void DefaultMoveTo(RaycastHit raycastHit) {
            DefaultMoveTo(raycastHit.point);
        }

        public void DefaultMoveTo(Vector3 destination) {
            switch (defaultMovementMode) {
                case PlayerMovementMode.NAVMESH_WALK:
                    WalkTo(destination);
                    break;
                case PlayerMovementMode.TELEPORT:
                    TeleportTo(destination);
                    break;
            }
        }

        public void WalkTo(RaycastHit raycastHit) {
            WalkTo(raycastHit.point);
        }

        public void WalkTo(Vector3 destination) {
			// Try to walk to the destination, if we fail and teleport fallback is enabled, teleport
            var path = new NavMeshPath();
			m_NavMeshAgent.CalculatePath(destination, path);
			if (path.status != NavMeshPathStatus.PathComplete)
                Debug.LogError("PlayerBody.MoveTo has received a destination beyond the NavMesh area");
            else
                m_NavMeshAgent.destination = destination;
        }

        public void TeleportTo(RaycastHit raycastHit) {
            TeleportTo(raycastHit.point);
        }

        public void TeleportTo(Vector3 destination) {
            m_NavMeshAgent.enabled = false;
            Runner fadeOutWork = Runner.New(cameraFader.BeginFadeOut(m_TeleportFadeDuration, null));
            fadeOutWork.Run(() => {
                transform.position = new Vector3(
                    destination.x,
                    destination.y,
                    destination.z
                );
                m_NavMeshAgent.enabled = true;
                m_NavMeshAgent.destination = destination;
                Runner fadeInWork = Runner.New(cameraFader.BeginFadeIn(m_TeleportFadeDuration, null));
                fadeInWork.Run();
            });
        }

        void Start() {
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
            m_CharacterController = GetComponent<CharacterController>();
            UpdateFromDimensions();
        }
    
        void UpdateFromDimensions() {
            m_NavMeshAgent.radius = radius;
            m_NavMeshAgent.height = height;

            m_CharacterController.height = height;
            m_CharacterController.radius = radius;
            m_CharacterController.center = new Vector3(0, height / 2, 0);
        }
    }
}

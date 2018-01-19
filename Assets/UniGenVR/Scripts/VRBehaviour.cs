using UnityEngine;
using UniGenVR.UI;
using UniGenVR.Utils;
using UniGenVR.Player;
using UniGenVR.Component;

namespace UniGenVR {
    public class VRBehaviour : MonoBehaviour {
        Interactable m_Interactable;
        public Interactable interactable {
            get {
                if (m_Interactable == null)
                    m_Interactable = GetComponent<Interactable>();
                return m_Interactable;
            }
        }

        EyeRaycaster m_VREyeRaycaster;
        public EyeRaycaster eyeRaycaster {
            get {
                if (m_VREyeRaycaster == null)
                    m_VREyeRaycaster = GetComponent<EyeRaycaster>();
                return m_VREyeRaycaster;
            }
        }

        CameraUI m_VRCameraUI;
        public CameraUI cameraUI {
            get {
                if (m_VRCameraUI == null)
                    m_VRCameraUI = GetComponent<CameraUI>();
                return cameraUI;
            }
        }

        Reticle m_VRReticle;
        public Reticle reticle {
            get {
                if (m_VRReticle == null)
                    m_VRReticle = GetComponent<Reticle>();
                return m_VRReticle;
            }
        }

        ReticleRadial m_VRReticleRadial;
        public ReticleRadial ReticleRadial {
            get {
                if (m_VRReticleRadial == null)
                    m_VRReticleRadial = GetComponent<ReticleRadial>();
                return m_VRReticleRadial;
            }
        }

        CameraFade m_VRCameraFade;
        public CameraFade vrCameraFade {
            get {
                if (m_VRCameraFade == null)
                    m_VRCameraFade = GetComponent<CameraFade>();
                return m_VRCameraFade;
            }
        }

        PlayerEntity m_PlayerEntiry;
        public PlayerEntity playerEntity {
            get {
                if (m_PlayerEntiry == null)
                    m_PlayerEntiry = GetComponent<PlayerEntity>();
                return m_PlayerEntiry;
            }
        }
    }
}

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

        EyeRaycaster m_EyeRaycaster;
        public EyeRaycaster eyeRaycaster {
            get {
                if (m_EyeRaycaster == null)
                    m_EyeRaycaster = GetComponent<EyeRaycaster>();
                return m_EyeRaycaster;
            }
        }

        CameraUI m_CameraUI;
        public CameraUI cameraUI {
            get {
                if (m_CameraUI == null)
                    m_CameraUI = GetComponent<CameraUI>();
                return cameraUI;
            }
        }

        Reticle m_Reticle;
        public Reticle reticle {
            get {
                if (m_Reticle == null)
                    m_Reticle = GetComponent<Reticle>();
                return m_Reticle;
            }
        }

        Radial m_ReticleRadial;
        public Radial reticleRadial {
            get {
                if (m_ReticleRadial == null)
                    m_ReticleRadial = GetComponent<Radial>();
                return m_ReticleRadial;
            }
        }

        CameraFader m_CameraFade;
        public CameraFader cameraFade {
            get {
                if (m_CameraFade == null)
                    m_CameraFade = GetComponent<CameraFader>();
                return m_CameraFade;
            }
        }

        PlayerEntity m_PlayerEntity;
        public PlayerEntity playerEntity {
            get {
                if (m_PlayerEntity == null)
                    m_PlayerEntity = GetComponent<PlayerEntity>();
                return m_PlayerEntity;
            }
        }
    }
}

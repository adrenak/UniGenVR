using UnityEngine;

namespace UniGenVR {
    public class VRBehaviour : MonoBehaviour {
        Interactable m_VRInteractiveItem;
        public Interactable interactiveItem {
            get {
                if (m_VRInteractiveItem == null)
                    m_VRInteractiveItem = GetComponent<Interactable>();
                return m_VRInteractiveItem;
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
        public ReticleRadial reticleRadial {
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
    }
}

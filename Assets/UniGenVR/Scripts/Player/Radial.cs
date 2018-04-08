using UnityEngine;
using UniGenVR.Utils;
using UnityEngine.UI;

namespace UniGenVR.Player {
    // This class is used to control a radial bar that fills
    // up. When it has finished filling it triggers an event.
    public class Radial : MonoBehaviour {
        // Whether or not the bar should be visible at the start.
        [SerializeField] bool m_HideOnStart = true;

        // Reference to the image who's fill amount is adjusted to display the bar.
        [SerializeField] Image m_Selection;

        // Whether or not the bar is currently useable.
        bool m_CanFill;

        private void OnEnable() {
            UGVRInput.OnDown += HandleDown;
            UGVRInput.OnUp += HandleUp;
            UGVRInput.OnHold += HandleHold;
            UGVRInput.OnMaxHold += HandleMaxHold;
        }

        private void OnDisable() {
            UGVRInput.OnDown -= HandleDown;
            UGVRInput.OnUp -= HandleUp;
            UGVRInput.OnHold -= HandleHold;
            UGVRInput.OnMaxHold -= HandleMaxHold;
        }

        void Start() {
            m_Selection.fillAmount = 0f;

            if (m_HideOnStart)
                Hide();
        }

        public void CanFill(bool value) {
            m_CanFill = value;
            if (m_CanFill == false)
                m_Selection.fillAmount = 0;
        }

        public void Show() {
            m_Selection.gameObject.SetActive(true);
        }

        public void Hide() {
            m_Selection.gameObject.SetActive(false);
            m_Selection.fillAmount = 0f;
        }

        public void HandleDown() {
            m_Selection.fillAmount = 0;
        }

        public void HandleHold(float obj) {
            if(m_CanFill)
                m_Selection.fillAmount = obj;
        }

        public void HandleMaxHold() {
            if (m_CanFill)
                m_Selection.fillAmount = 1;
        }
        
        public void HandleUp() {
            m_Selection.fillAmount = 0f;
        }
    }
}
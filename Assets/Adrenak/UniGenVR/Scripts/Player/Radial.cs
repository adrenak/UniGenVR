using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UniGenVR {
    // This class is used to control a radial bar that fills
    // up. When it has finished filling it triggers an event.
    public class Radial : MonoBehaviour {
        // Whether or not the bar should be visible at the start.
        [SerializeField] bool m_HideOnStart = true;

        // Reference to the image who's fill amount is adjusted to display the bar.
        [SerializeField] Image m_Selection;
        
        void Start() {
            m_Selection.fillAmount = 0f;

            if (m_HideOnStart)
                Hide();
        }

        public void CanFill(bool value) {
            m_Selection.fillAmount = 0;
        }

        public void Show() {
            m_Selection.gameObject.SetActive(true);
        }

        public void Hide() {
            m_Selection.gameObject.SetActive(false);
        }

        public void SetFill(float fill) {
            m_Selection.fillAmount = fill;
        }

        public void Empty() {
            SetFill(0);
        }

        public void Full() {
            SetFill(1);
        }
    }
}
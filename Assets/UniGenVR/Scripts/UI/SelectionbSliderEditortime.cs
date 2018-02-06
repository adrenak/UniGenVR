using UnityEngine;

namespace UniGenVR.UI {
    [ExecuteInEditMode]
    public class SelectionbSliderEditortime : MonoBehaviour {
        BoxCollider m_BoxCollider;
        RectTransform m_RectTransform;

        private void Start() {
            m_RectTransform = GetComponent<RectTransform>();
            m_BoxCollider = GetComponent<BoxCollider>();
        }

        void Update () {
            m_BoxCollider.size = new Vector3(
                m_RectTransform.sizeDelta.x,
                m_RectTransform.sizeDelta.y,
                .01F
            );
        }
    }
}

using UnityEngine;

namespace UniGenVR {
    // This is a simple script to tint UI Images a colour when looked at.
    public class Tint : MonoBehaviour {
        // The colour to tint the Images.
        [SerializeField] private Color m_Tint;

        // How much the colour should be used.
        [Range(0f, 1f)] [SerializeField] private float m_TintPercent = 0.5f;

        // References to the images which will be tinted.
        [SerializeField] private UnityEngine.UI.Image[] m_ImagesToTint;
        
        public void HandleOver() {
            // When the user looks at the VRInteractiveItem go through all the images...
            for (int i = 0; i < m_ImagesToTint.Length; i++) {
                // and ADD to their colour by an amount based on the tint percentage.  Note this will push the colour closer to white.
                m_ImagesToTint[i].color += m_Tint * m_TintPercent;
            }
        }


        public void HandleOut() {
            // When the user looks away from the VRInteractiveItem go through all the images...
            for (int i = 0; i < m_ImagesToTint.Length; i++) {
                // ...and subtract the same amount.
                m_ImagesToTint[i].color -= m_Tint * m_TintPercent;
            }
        }
    }
}
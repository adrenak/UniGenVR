using UnityEngine;

namespace Adrenak.UniGenVR {
    [ExecuteInEditMode]
    public class PlayerHeadRoot : MonoBehaviour {
        PlayerBody m_Body;

        void OnEnable() {
            if (m_Body == null) 
                m_Body = GameObject.FindObjectOfType<PlayerBody>();
        }

        void Update() {
            // Do not allow rotation or scaling
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            transform.position = m_Body.transform.position + Vector3.up * m_Body.height;
        }
    }
}

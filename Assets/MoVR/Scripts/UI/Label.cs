using UnityEngine;

namespace UniGenVR {
    public class Label : MonoBehaviour {
        [SerializeField] UnityEngine.UI.Text text;

        public void Show() {
            gameObject.SetActive(true);
        }

        public void Hide() {
            gameObject.SetActive(false);
        }

        public void SetText(string txt) {
            text.text = txt;
        }
    }
}

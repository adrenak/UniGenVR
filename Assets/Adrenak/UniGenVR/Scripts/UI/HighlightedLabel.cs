using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UniGenVR {
    public class HighlightedLabel : MonoBehaviour {
        [SerializeField] Image highlight;
        [SerializeField] Text text;

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

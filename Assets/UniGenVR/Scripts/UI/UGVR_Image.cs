using UnityEngine;
using UnityEngine.UI;

namespace UniGenVR.UI {
    public class UGVR_Image : MonoBehaviour {
        [SerializeField] Image image;

        public void Show() {
            gameObject.SetActive(true);
        }

        public void Hide() {
            gameObject.SetActive(false);
        }

        public void SetImage(Sprite sprite) {
            image.sprite = sprite;
        }
    }
}

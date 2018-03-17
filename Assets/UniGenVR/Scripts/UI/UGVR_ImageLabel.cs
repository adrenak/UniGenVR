using UnityEngine;
using UnityEngine.UI;

public class UGVR_ImageLabel : MonoBehaviour {
    [SerializeField] Image image;
    [SerializeField] Text text;

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void SetImage(Sprite sprite) {
        image.sprite = sprite;
    }

    public void SetLabel(string label) {
        text.text = label;
    }
}

using UnityEngine;
using UnityEditor;
using UniGenVR.Player;
using UniGenVR.Utils;

namespace UniGenVR.Ed {
    public class EditorTools : MonoBehaviour {
        [MenuItem("UniGenVR/Select Player Body")]
        static void SelectUGCVRPlayerBody() {
            var body = GameObject.FindObjectOfType<PlayerBody>();
            if (body != null)
                Selection.activeGameObject = body.gameObject;
        }

        [MenuItem("UniGenVR/Select Player Head")]
        static void SelectUGCVRPlayerHead() {
            var head = GameObject.FindObjectOfType<PlayerHead>();
            if (head != null)
                Selection.activeGameObject = head.gameObject;
        }

        [MenuItem("UniGenVR/Select Utils")]
        static void SelectUGCVRUtils() {
            var utils = GameObject.FindObjectOfType<UtilsEntity>();
            if (utils != null)
                Selection.activeGameObject = utils.gameObject;
        }
    }
}
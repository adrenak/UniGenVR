using UnityEngine;

namespace UniGenVR.Utils {
    public class UtilsEntity : MonoBehaviour {
	    void Awake () {
            DontDestroyOnLoad(gameObject);
	    }
    }
}

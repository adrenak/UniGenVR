using UnityEngine;

namespace UniGenVR {
    public class UtilsEntity : MonoBehaviour {
	    void Awake () {
            DontDestroyOnLoad(gameObject);
	    }
    }
}

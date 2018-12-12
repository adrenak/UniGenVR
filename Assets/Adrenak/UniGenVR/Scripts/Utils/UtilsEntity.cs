using UnityEngine;

namespace Adrenak.UniGenVR {
    public class UtilsEntity : MonoBehaviour {
	    void Awake () {
            DontDestroyOnLoad(gameObject);
	    }
    }
}

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Adrenak.UniGenVR {
    // This class is used to fade the entire screen to black (or
    // any chosen colour).  It should be used to smooth out the
    // transition between scenes or restarting of a scene.
    public class Fader : MonoBehaviour {
        // This is called when the fade in or out has finished.

        // The colour the image fades out to.
        public Color fadeColor = Color.black;

        // Reference to the image that covers the screen.
        [SerializeField] Image m_FadeImage;

        // How long it takes to fade in seconds.
        [SerializeField] float m_DefaultFadeDuration = 2.0f;

        // Whether a fade in should happen as soon as the scene is loaded.
        [SerializeField] bool m_FadeInOnSceneLoad = false;

        // Whether a fade in should happen just but Updates start.
        [SerializeField] bool m_FadeInOnStart = false;

        // Whether the screen is currently fading.
        bool m_IsFading;

        // The time when fading started.
        float m_FadeStartTime;

        // This is a transparent version of the fade colour, it will ensure fading looks normal.
        Color m_FadeColorTrans;

        public bool IsFading { get { return m_IsFading; } }

        void Awake() {
            SceneManager.sceneLoaded += HandleSceneLoaded;

            m_FadeColorTrans = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0f);
            m_FadeImage.enabled = true;
        }

        void Start() {
            // If applicable set the immediate colour to be faded out and then fade in.
            if (m_FadeInOnStart) {
                m_FadeImage.color = fadeColor;
                FadeIn(null);
            }
        }

        void HandleSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
            // If applicable set the immediate colour to be faded out and then fade in.
            if (m_FadeInOnSceneLoad) {
                m_FadeImage.color = fadeColor;
                FadeIn(null);
            }
        }

        // Since no duration is specified with this overload use the default duration.
        public void FadeOut(Action callback) {
            FadeOut(m_DefaultFadeDuration, null);
        }

        public void FadeOut(float duration, Action callback) {
            // If not already fading start a coroutine to fade from the fade out colour to the fade colour.
            if (m_IsFading)
                return;
            StartCoroutine(BeginFade(m_FadeColorTrans, fadeColor, duration, callback));
        }

        // Since no duration is specified with this overload use the default duration.
        public void FadeIn(Action callback) {
            FadeIn(m_DefaultFadeDuration, callback);
        }
        
        public void FadeIn(float duration, Action callback) {
            // If not already fading start a coroutine to fade from the fade colour to the fade out colour.
            if (m_IsFading)
                return;
            StartCoroutine(BeginFade(fadeColor, m_FadeColorTrans, duration, callback));
        }
        
        public IEnumerator BeginFadeOut(Action callback) {
            yield return StartCoroutine(BeginFade(m_FadeColorTrans, fadeColor, m_DefaultFadeDuration, callback));
        }
        
        public IEnumerator BeginFadeOut(float duration, Action callback) {
            yield return StartCoroutine(BeginFade(m_FadeColorTrans, fadeColor, duration, callback));
        }
        
        public IEnumerator BeginFadeIn(Action callback) {
            yield return StartCoroutine(BeginFade(fadeColor, m_FadeColorTrans, m_DefaultFadeDuration, callback));
        }
        
        public IEnumerator BeginFadeIn(float duration, Action callback) {
            yield return StartCoroutine(BeginFade(fadeColor, m_FadeColorTrans, duration, callback));
        }
        
        private IEnumerator BeginFade(Color startCol, Color endCol, float duration, Action callback) {
            // Fading is now happening.  This ensures it won't be interupted by non-coroutine calls.
            m_IsFading = true;

            // Execute this loop once per frame until the timer exceeds the duration.
            float timer = 0f;
            while (timer <= duration) {
                // Set the colour based on the normalised time.
                m_FadeImage.color = Color.Lerp(startCol, endCol, timer / duration);

                // Increment the timer by the time between frames and return next frame.
                timer += Time.deltaTime;
                yield return null;
            }

            // Fading is finished so allow other fading calls again.
            m_IsFading = false;

            if (callback != null)
                callback();
        }

        void OnDestroy() {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
        }
    }
}
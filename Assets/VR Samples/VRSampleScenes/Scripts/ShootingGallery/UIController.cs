using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniGenVR.Common;
using UniGenVR.Utils;

namespace UniGenVR.ShootingGallery
{
    // This simple class encapsulates the UI for
    // the shooter scenes so that the game
    // controller need only reference one thing to
    // control the UI during the games.
    public class UIController : MonoBehaviour
    {
        [SerializeField] private ScreenFader m_IntroUI;     // This controls fading the UI shown during the intro.
        [SerializeField] private ScreenFader m_OutroUI;     // This controls fading the UI shown during the outro.
        [SerializeField] private ScreenFader m_PlayerUI;    // This controls fading the UI that shows around the gun that moves with the player.
        [SerializeField] private Text m_TotalScore;     // Reference to the Text component that displays the player's score at the end.
        [SerializeField] private Text m_HighScore;      // Reference to the Text component that displays the high score at the end.


        public IEnumerator ShowIntroUI()
        {
            yield return StartCoroutine(m_IntroUI.InteruptAndFadeIn());
        }


        public IEnumerator HideIntroUI()
        {
            yield return StartCoroutine(m_IntroUI.InteruptAndFadeOut());
        }


        public IEnumerator ShowOutroUI()
        {
            m_TotalScore.text = SessionData.Score.ToString();
            m_HighScore.text = SessionData.HighScore.ToString();

            yield return StartCoroutine(m_OutroUI.InteruptAndFadeIn());
        }


        public IEnumerator HideOutroUI()
        {
            yield return StartCoroutine(m_OutroUI.InteruptAndFadeOut());
        }


        public IEnumerator ShowPlayerUI ()
        {
            yield return StartCoroutine (m_PlayerUI.InteruptAndFadeIn ());
        }


        public IEnumerator HidePlayerUI ()
        {
            yield return StartCoroutine (m_PlayerUI.InteruptAndFadeOut ());
        }
    }
}
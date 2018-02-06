using System;
using UnityEngine;
using UniGenVR.Utils;
using UnityEngine.UI;
using UniGenVR.Component;

namespace UniGenVR.UI {
    [RequireComponent(typeof(Interactable))]
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(AudioSource))]
    public class SelectionSlider : VRBehaviour {
        public event Action OnSliderFilled;                                    // This event is triggered when the bar finishes filling.

        [SerializeField] bool m_DisableOnBarFill;                   // Whether the bar should stop reacting once it's been filled (for single use bars).
        [SerializeField] AudioClip m_OnOverClip;                    // The clip to play when the user looks at the bar.
        [SerializeField] AudioClip m_OnFilledClip;                  // The clip to play when the bar finishes filling.

        Slider m_Slider;
        UIFader m_UIFader;
        Collider m_Collider;
        AudioSource m_Audio;

        bool m_GazeOver;                                            // Whether the user is currently looking at the bar.
        bool m_Hold;
        float m_Timer;
       
        private void Start() {
            m_Audio = GetComponent<AudioSource>();
            m_Slider = transform.GetChild(0).GetComponent<Slider>();
            m_Collider = GetComponent<BoxCollider>();
            m_UIFader = GetComponentInParent<UIFader>();
        }

        private void OnEnable() {
            VRInput.OnUp += HandleUp;
            VRInput.OnDown += HandleDown;

            interactable.OnOver += HandleOver;
            interactable.OnOut += HandleOut;
        }

        private void HandleDown() {
            m_Hold = true;
        }

        private void OnDisable() {
            VRInput.OnUp -= HandleUp;
            VRInput.OnDown -= HandleDown;

            interactable.OnOver -= HandleOver;
            interactable.OnOut -= HandleOut;
        }

        private void Update() {
            // If this bar is using a UIFader turn off the collider when it's invisible.
            if (m_UIFader && m_Collider)
                m_Collider.enabled = m_UIFader.Visible;

            // Update timer
            if (m_GazeOver && m_Hold)
                m_Timer += Time.deltaTime;
            else
                m_Timer = 0;

            SetSliderValue(m_Timer / VRInput.MaxHoldTime);
            if (m_Timer > VRInput.MaxHoldTime)
                SliderFilled();
        }

        void SetSliderValue(float value) {
            if (m_Slider)
                m_Slider.value = value;
        }
        
        private void SliderFilled() {
            // If anything has subscribed to OnBarFilled call it now.
            if (OnSliderFilled != null)
                OnSliderFilled();

            // Play the clip for when the bar is filled.
            m_Audio.clip = m_OnFilledClip;
            m_Audio.Play();


            // If the bar should be disabled once it is filled, do so now.
            if (m_DisableOnBarFill)
                enabled = false;

            // Reset stuff
            m_Timer = 0;
            m_Hold = false;
            m_GazeOver = false;
            SetSliderValue(0);
        }
                
        private void HandleUp() {
            m_Hold = false;
        }
        
        private void HandleOver() {
            m_GazeOver = true;
            m_Audio.clip = m_OnOverClip;
            m_Audio.Play();
        }
        
        private void HandleOut() {
            m_GazeOver = false;

            SetSliderValue(0f);
        }
    }
}
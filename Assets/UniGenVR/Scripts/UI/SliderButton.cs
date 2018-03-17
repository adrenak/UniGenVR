using System;
using UnityEngine;
using UniGenVR.Utils;
using UnityEngine.UI;
using UniGenVR.Component;

namespace UniGenVR.UI {
    [RequireComponent(typeof(Interactable))]
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(AudioSource))]
    public class SliderButton : VRBehaviour {
        public event Action OnSliderFilled;

        [SerializeField] Text label;
        [SerializeField] bool m_DisableOnBarFill;
        [SerializeField] AudioClip m_OnOverClip;
        [SerializeField] AudioClip m_OnOutClip;
        [SerializeField] AudioClip m_OnFilledClip;

        Slider m_Slider;
        UIFader m_UIFader;
        Collider m_Collider;
        AudioSource m_Audio;

        bool m_Fill;
        float m_Timer;
       
        public void SetText(string text) {
            label.text = text;
        }

        private void Start() {
            m_Audio = GetComponent<AudioSource>();
            m_Slider = transform.GetChild(0).GetComponent<Slider>();
            m_Collider = GetComponent<BoxCollider>();
            m_UIFader = GetComponentInParent<UIFader>();
        }

        private void OnEnable() {
            interactable.OnOver += HandleOver;
            interactable.OnOut += HandleOut;
            interactable.OnDown += HandleDown;
            interactable.OnUp += HandleUp;
        }
        
        private void OnDisable() {
            interactable.OnOver -= HandleOver;
            interactable.OnOut -= HandleOut;
            interactable.OnDown -= HandleDown;
            interactable.OnUp -= HandleUp;
        }

        private void Update() {
            // TODO: This has bugs
            // If this bar is using a UIFader turn off the collider when it's invisible.
            if (m_UIFader)
                m_Collider.enabled = m_UIFader.Visible;

            // Update timer
            if (m_Fill)
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
            TryPlayAudioClip(m_OnFilledClip);

            // If the bar should be disabled once it is filled, do so now.
            if (m_DisableOnBarFill) enabled = false;

            // Reset stuff
            m_Timer = 0;
            m_Fill = false;
            SetSliderValue(0);
        }
                
        private void HandleUp() {
            m_Fill = false;
        }
        
        private void HandleDown() {
            m_Fill = true;
        }

        private void HandleOver() {
            TryPlayAudioClip(m_OnOverClip);
        }
        
        private void HandleOut() {
            m_Fill = false;
            TryPlayAudioClip(m_OnOutClip);
        }

        void TryPlayAudioClip(AudioClip clip) {
            if (clip == null) return;
            m_Audio.clip = clip;
            m_Audio.Play();
        }
    }
}
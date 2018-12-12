using System;
using UnityEngine;

namespace Adrenak.UniGenVR {
    // This class encapsulates all the input required for most VR games.
    // It has events that can be subscribed to by classes that need specific input.
    // This class must exist in every scene and so can be attached to the main
    // camera for ease.
    public class UGVRInput : MonoBehaviour {
        static UGVRInput instance;
        public static UGVRInput Instance {
            get { return instance; }
        }

        public enum SwipeDirection {
            NONE,
            UP,
            DOWN,
            LEFT,
            RIGHT
        };

        public static event Action<SwipeDirection> OnSwipe;                // Called every frame passing in the swipe, including if there is no swipe.
        public static event Action<float> OnHold;
        public static event Action OnMaxHold;
        public static event Action OnClick;                                // Called when Fire1 is released and it's not a double click.
        public static event Action OnDown;                                 // Called when Fire1 is pressed.
        public static event Action OnUp;                                   // Called when Fire1 is released.
        public static event Action OnDoubleClick;                          // Called when a double click is detected.
        public static event Action OnCancel;                               // Called when Cancel is pressed.

        [SerializeField] KeyCode m_TriggerKey;
        [SerializeField] KeyCode m_CancelKey;

        [SerializeField] float m_MaxHoldTime = 2;
        [SerializeField] float m_MaxGazeTime = 2;
        [SerializeField] float m_MinHoldTime = .2F;
        [SerializeField] float m_DoubleClickTime = 0.3f;
        [SerializeField] float m_SwipeDistance = 0.3f;         //The width of a swipe

        private Vector2 m_MouseDownPosition;                        // The screen position of the mouse when Fire1 is pressed.
        private Vector2 m_MouseUpPosition;                          // The screen position of the mouse when Fire1 is released.
        private float m_LastMouseUpTime;                            // The time when Fire1 was last released.
        private float m_LastMouseDownTime;                            // The time when Fire1 was last released.
        private float m_LastHorizontalValue;                        // The previous value of the horizontal axis used to detect keyboard swipes.
        private float m_LastVerticalValue;                          // The previous value of the vertical axis used to detect keyboard swipes.

        public static float DoubleClickInterval { get { return instance.m_DoubleClickTime; } }
        public static float MaxHoldTime { get { return instance.m_MaxHoldTime; } }
        public static float MaxGazeTime { get { return instance.m_MaxGazeTime; } }
        public static float MinHoldTime { get { return instance.m_MinHoldTime; } }
        public static float SwipeDistance { get { return instance.m_SwipeDistance; } }

        private void Awake() {
            var instances = FindObjectsOfType<UGVRInput>();
            if (instances.Length > 1)
                Destroy(gameObject);
            else
                instance = gameObject.GetComponent<UGVRInput>();
        }

        private void Update() {
            CheckInput();
        }

        private void CheckInput() {
            // Set the default swipe to be none.
            SwipeDirection swipe = SwipeDirection.NONE;

            // TRIGGER DOWN
            if (Input.GetKeyDown(m_TriggerKey)) {
                // When Fire1 is pressed record the position of the mouse.
                m_MouseDownPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                m_LastMouseDownTime = Time.time;

                // If anything has subscribed to OnDown call it.
                if (OnDown != null)
                    OnDown();
            }


            // TRIGGER UP AND DOUBLE CLICK
            if (Input.GetKeyUp(m_TriggerKey)) {
                // If anything has subscribed to OnUp call it.
                if (OnUp != null)
                    OnUp();

                // If the time between the last release of Fire1 and now is less
                // than the allowed double click time then it's a double click.
                if (Time.time - m_LastMouseUpTime < m_DoubleClickTime) {
                    // If anything has subscribed to OnDoubleClick call it.
                    if (OnDoubleClick != null)
                        OnDoubleClick();
                }
                else {
                    // If it's not a double click, it's a single click.
                    // If anything has subscribed to OnClick call it.
                    if (OnClick != null)
                        OnClick();
                }

                // Record the time when Fire1 is released.
                m_LastMouseUpTime = Time.time;
            }

            // TRIGGER HOLD
            if (Input.GetKey(m_TriggerKey)) {
                var holdTime = (Time.time - m_LastMouseDownTime);
                var normalizedHoldTime = holdTime / m_MaxHoldTime;

                if(OnHold != null && holdTime > m_MinHoldTime)
                    OnHold(normalizedHoldTime);
                if(OnMaxHold != null && normalizedHoldTime > 1) {
                    OnMaxHold();
                    m_LastMouseDownTime = Time.time;
                }
            }

            // CANCEL
            if (Input.GetKeyDown(m_CancelKey)) {
                if (OnCancel != null)
                    OnCancel();
            }

            // SWIPE
            if (Input.GetKeyUp(m_TriggerKey)) {
                // When Fire1 is released record the position of the mouse.
                m_MouseUpPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

                // Detect the direction between the mouse positions when Fire1 is pressed and released.
                swipe = DetectSwipe();
            }

            // If there was no swipe this frame from the mouse, check for a keyboard swipe.
            if (swipe == SwipeDirection.NONE)
                swipe = DetectKeyboardEmulatedSwipe();

            // If there are any subscribers to OnSwipe call it passing in the detected swipe.
            if (OnSwipe != null)
                OnSwipe(swipe);
        }


        private SwipeDirection DetectSwipe() {
            // Get the direction from the mouse position when Fire1 is pressed to when it is released.
            Vector2 swipeData = (m_MouseUpPosition - m_MouseDownPosition).normalized;

            // If the direction of the swipe has a small width it is vertical.
            bool swipeIsVertical = Mathf.Abs(swipeData.x) < m_SwipeDistance;

            // If the direction of the swipe has a small height it is horizontal.
            bool swipeIsHorizontal = Mathf.Abs(swipeData.y) < m_SwipeDistance;

            // If the swipe has a positive y component and is vertical the swipe is up.
            if (swipeData.y > 0f && swipeIsVertical)
                return SwipeDirection.UP;

            // If the swipe has a negative y component and is vertical the swipe is down.
            if (swipeData.y < 0f && swipeIsVertical)
                return SwipeDirection.DOWN;

            // If the swipe has a positive x component and is horizontal the swipe is right.
            if (swipeData.x > 0f && swipeIsHorizontal)
                return SwipeDirection.RIGHT;

            // If the swipe has a negative x component and is vertical the swipe is left.
            if (swipeData.x < 0f && swipeIsHorizontal)
                return SwipeDirection.LEFT;

            // If the swipe meets none of these requirements there is no swipe.
            return SwipeDirection.NONE;
        }


        private SwipeDirection DetectKeyboardEmulatedSwipe() {
            // Store the values for Horizontal and Vertical axes.
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // Store whether there was horizontal or vertical input before.
            bool noHorizontalInputPreviously = Mathf.Abs(m_LastHorizontalValue) < float.Epsilon;
            bool noVerticalInputPreviously = Mathf.Abs(m_LastVerticalValue) < float.Epsilon;

            // The last horizontal values are now the current ones.
            m_LastHorizontalValue = horizontal;
            m_LastVerticalValue = vertical;

            // If there is positive vertical input now and previously there wasn't the swipe is up.
            if (vertical > 0f && noVerticalInputPreviously)
                return SwipeDirection.UP;

            // If there is negative vertical input now and previously there wasn't the swipe is down.
            if (vertical < 0f && noVerticalInputPreviously)
                return SwipeDirection.DOWN;

            // If there is positive horizontal input now and previously there wasn't the swipe is right.
            if (horizontal > 0f && noHorizontalInputPreviously)
                return SwipeDirection.RIGHT;

            // If there is negative horizontal input now and previously there wasn't the swipe is left.
            if (horizontal < 0f && noHorizontalInputPreviously)
                return SwipeDirection.LEFT;

            // If the swipe meets none of these requirements there is no swipe.
            return SwipeDirection.NONE;
        }


        private void OnDestroy() {
            // Ensure that all events are unsubscribed when this is destroyed.
            OnSwipe = null;
            OnClick = null;
            OnDoubleClick = null;
            OnDown = null;
            OnUp = null;
        }
    }
}
using UnityEngine;

namespace DriftRacer.Input
{
    public class ButtonInputProvider : MonoBehaviour, IInputProvider
    {
        [Header("Button States")]
        private bool leftButtonPressed = false;
        private bool rightButtonPressed = false;
        private bool handbrakeButtonPressed = false;
        private bool brakeButtonPressed = false;

        [Header("Keyboard Fallback")]
        [SerializeField] private bool useKeyboardFallback = true;

        public void SetLeftButton(bool pressed)
        {
            leftButtonPressed = pressed;
        }

        public void SetRightButton(bool pressed)
        {
            rightButtonPressed = pressed;
        }

        public void SetHandbrakeButton(bool pressed)
        {
            handbrakeButtonPressed = pressed;
        }

        public void SetBrakeButton(bool pressed)
        {
            brakeButtonPressed = pressed;
        }

        public float GetSteering()
        {
            float steering = 0f;

            if (leftButtonPressed)
                steering -= 1f;

            if (rightButtonPressed)
                steering += 1f;

            if (useKeyboardFallback && Application.isEditor)
            {
                if (UnityEngine.Input.GetKey(KeyCode.LeftArrow) || UnityEngine.Input.GetKey(KeyCode.A))
                    steering -= 1f;

                if (UnityEngine.Input.GetKey(KeyCode.RightArrow) || UnityEngine.Input.GetKey(KeyCode.D))
                    steering += 1f;
            }

            return Mathf.Clamp(steering, -1f, 1f);
        }

        public bool IsHandbrakePressed()
        {
            bool pressed = handbrakeButtonPressed;

            if (useKeyboardFallback && Application.isEditor)
            {
                pressed |= UnityEngine.Input.GetKey(KeyCode.Space);
            }

            return pressed;
        }

        public bool IsBrakePressed()
        {
            bool pressed = brakeButtonPressed;

            if (useKeyboardFallback && Application.isEditor)
            {
                pressed |= UnityEngine.Input.GetKey(KeyCode.DownArrow) || UnityEngine.Input.GetKey(KeyCode.S);
            }

            return pressed;
        }
    }
}

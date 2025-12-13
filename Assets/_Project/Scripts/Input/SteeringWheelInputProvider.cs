using UnityEngine;

namespace DriftRacer.Input
{
    public class SteeringWheelInputProvider : MonoBehaviour, IInputProvider
    {
        [Header("Steering Wheel State")]
        [SerializeField] private float currentSteering = 0f;
        [SerializeField] private bool handbrakePressed = false;

        [Header("Settings")]
        [SerializeField] private float steeringSensitivity = 2f;
        [SerializeField] private float returnSpeed = 5f;

        public void SetSteering(float value)
        {
            currentSteering = Mathf.Clamp(value, -1f, 1f);
        }

        public void SetHandbrake(bool pressed)
        {
            handbrakePressed = pressed;
        }

        private void Update()
        {
            if (Mathf.Abs(currentSteering) > 0.01f)
            {
                currentSteering = Mathf.Lerp(currentSteering, 0f, returnSpeed * Time.deltaTime);
            }
        }

        public float GetSteering()
        {
            return currentSteering;
        }

        public bool IsHandbrakePressed()
        {
            return handbrakePressed;
        }

        public bool IsBrakePressed()
        {
            return false;
        }
    }
}

using UnityEngine;
using DriftRacer.Data;
using System;

namespace DriftRacer.Car
{
    [RequireComponent(typeof(CarPhysics))]
    public class DriftController : MonoBehaviour
    {
        [Header("References")]
        private CarPhysics carPhysics;
        private CarData carData;

        [Header("Drift State")]
        [SerializeField] private bool isDrifting = false;
        [SerializeField] private float driftTime = 0f;
        [SerializeField] private float currentDriftAngle = 0f;
        [SerializeField] private float driftQuality = 0f;

        [Header("Drift Settings")]
        [SerializeField] private float perfectDriftAngleMin = 25f;
        [SerializeField] private float perfectDriftAngleMax = 45f;

        [Header("Input")]
        private bool handbrakePressed = false;

        public bool IsDrifting => isDrifting;
        public float DriftTime => driftTime;
        public float DriftAngle => currentDriftAngle;
        public float DriftQuality => driftQuality;

        public event Action<float> OnDriftScore;
        public event Action OnDriftStart;
        public event Action OnDriftEnd;

        private void Awake()
        {
            carPhysics = GetComponent<CarPhysics>();
        }

        public void Initialize(CarData data)
        {
            carData = data;
        }

        public void SetHandbrake(bool pressed)
        {
            handbrakePressed = pressed;
        }

        private void Update()
        {
            if (carData == null) return;

            UpdateDriftState();
        }

        private void UpdateDriftState()
        {
            currentDriftAngle = carPhysics.GetDriftAngle();
            float speed = carPhysics.CurrentSpeed;

            bool shouldDrift = handbrakePressed &&
                               speed >= carData.minDriftSpeed &&
                               Mathf.Abs(currentDriftAngle) >= carData.minDriftAngle;

            if (shouldDrift && !isDrifting)
            {
                StartDrift();
            }
            else if (!shouldDrift && isDrifting)
            {
                EndDrift();
            }

            if (isDrifting)
            {
                UpdateDrift();
            }
        }

        private void StartDrift()
        {
            isDrifting = true;
            driftTime = 0f;

            carPhysics.SetFriction(carData.driftFriction);
            carPhysics.SetTurnRate(carData.driftTurnRate);

            OnDriftStart?.Invoke();
        }

        private void UpdateDrift()
        {
            driftTime += Time.deltaTime;

            driftQuality = CalculateDriftQuality();

            float scoreThisFrame = driftQuality * Time.deltaTime;
            OnDriftScore?.Invoke(scoreThisFrame);
        }

        private void EndDrift()
        {
            isDrifting = false;
            driftTime = 0f;
            driftQuality = 0f;

            carPhysics.SetFriction(carData.normalFriction);
            carPhysics.SetTurnRate(carData.turnRate);

            OnDriftEnd?.Invoke();
        }

        private float CalculateDriftQuality()
        {
            float angleQuality = 0f;
            float absAngle = Mathf.Abs(currentDriftAngle);

            if (absAngle >= perfectDriftAngleMin && absAngle <= perfectDriftAngleMax)
            {
                angleQuality = 1f;
            }
            else if (absAngle < perfectDriftAngleMin)
            {
                angleQuality = absAngle / perfectDriftAngleMin;
            }
            else
            {
                float overAngle = absAngle - perfectDriftAngleMax;
                angleQuality = Mathf.Max(0f, 1f - (overAngle / 30f));
            }

            float speedQuality = Mathf.Clamp01(carPhysics.CurrentSpeed / carData.maxSpeed);

            float quality = angleQuality * 0.7f + speedQuality * 0.3f;

            quality *= 100f;

            return quality;
        }

        public void ResetDrift()
        {
            if (isDrifting)
            {
                EndDrift();
            }
            handbrakePressed = false;
        }
    }
}

using UnityEngine;
using DriftRacer.Data;
using DriftRacer.Input;

namespace DriftRacer.Car
{
    [RequireComponent(typeof(CarPhysics))]
    [RequireComponent(typeof(DriftController))]
    public class CarController : MonoBehaviour
    {
        [Header("Car Setup")]
        [SerializeField] private CarData carData;

        [Header("References")]
        private CarPhysics carPhysics;
        private DriftController driftController;
        private SpriteRenderer spriteRenderer;

        [Header("Input")]
        private float steeringInput = 0f;
        private float throttleInput = 1f;
        private bool handbrakeInput = false;
        private bool brakeInput = false;

        [Header("Visual")]
        [SerializeField] private GameObject driftParticles;
        [SerializeField] private TrailRenderer[] tireMarks;

        public CarPhysics Physics => carPhysics;
        public DriftController Drift => driftController;
        public CarData Data => carData;

        private void Awake()
        {
            carPhysics = GetComponent<CarPhysics>();
            driftController = GetComponent<DriftController>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            if (spriteRenderer == null)
            {
                spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            }
        }

        private void Start()
        {
            if (carData != null)
            {
                Initialize(carData);
            }
        }

        public void Initialize(CarData data)
        {
            carData = data;
            carPhysics.Initialize(data);
            driftController.Initialize(data);

            if (spriteRenderer != null && data.carSprite != null)
            {
                spriteRenderer.sprite = data.carSprite;
                spriteRenderer.color = data.primaryColor;
            }

            driftController.OnDriftStart += OnDriftStarted;
            driftController.OnDriftEnd += OnDriftEnded;

            if (driftParticles != null)
            {
                driftParticles.SetActive(false);
            }
        }

        private void Update()
        {
            if (InputManager.Instance != null)
            {
                steeringInput = InputManager.Instance.GetSteering();
                handbrakeInput = InputManager.Instance.IsHandbrakePressed();
            }

            driftController.SetHandbrake(handbrakeInput);
        }

        private void FixedUpdate()
        {
            carPhysics.SetInput(throttleInput, steeringInput, brakeInput);
        }

        private void OnDriftStarted()
        {
            if (driftParticles != null)
            {
                driftParticles.SetActive(true);
            }

            if (tireMarks != null)
            {
                foreach (var mark in tireMarks)
                {
                    if (mark != null)
                    {
                        mark.emitting = true;
                    }
                }
            }
        }

        private void OnDriftEnded()
        {
            if (driftParticles != null)
            {
                driftParticles.SetActive(false);
            }

            if (tireMarks != null)
            {
                foreach (var mark in tireMarks)
                {
                    if (mark != null)
                    {
                        mark.emitting = false;
                    }
                }
            }
        }

        public void SetThrottle(float value)
        {
            throttleInput = Mathf.Clamp(value, 0f, 1f);
        }

        public void ResetCar()
        {
            carPhysics.ResetPhysics();
            driftController.ResetDrift();
            throttleInput = 1f;
            steeringInput = 0f;
            handbrakeInput = false;
            brakeInput = false;
        }

        private void OnDestroy()
        {
            if (driftController != null)
            {
                driftController.OnDriftStart -= OnDriftStarted;
                driftController.OnDriftEnd -= OnDriftEnded;
            }
        }
    }
}

using UnityEngine;
using DriftRacer.Data;

namespace DriftRacer.Car
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CarPhysics : MonoBehaviour
    {
        [Header("References")]
        private Rigidbody2D rb;
        private CarData carData;

        [Header("Current State")]
        [SerializeField] private float currentSpeed = 0f;
        [SerializeField] private float currentTurnRate = 100f;
        private float currentFriction = 0.5f;

        [Header("Input Values")]
        private float throttleInput = 0f;
        private float steeringInput = 0f;
        private bool isBraking = false;

        public float CurrentSpeed => currentSpeed;
        public Vector2 Velocity => rb.velocity;
        public float CurrentFriction => currentFriction;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.drag = 0f;
            rb.angularDrag = 0f;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        }

        public void Initialize(CarData data)
        {
            carData = data;
            rb.mass = data.mass;
            currentFriction = data.normalFriction;
            currentTurnRate = data.turnRate;
        }

        public void SetInput(float throttle, float steering, bool brake)
        {
            throttleInput = Mathf.Clamp(throttle, -1f, 1f);
            steeringInput = Mathf.Clamp(steering, -1f, 1f);
            isBraking = brake;
        }

        public void SetFriction(float friction)
        {
            currentFriction = friction;
        }

        public void SetTurnRate(float turnRate)
        {
            currentTurnRate = turnRate;
        }

        private void FixedUpdate()
        {
            if (carData == null) return;

            ApplyAcceleration();
            ApplySteering();
            ApplyFriction();
            UpdateSpeed();
        }

        private void ApplyAcceleration()
        {
            if (Mathf.Abs(throttleInput) > 0.01f)
            {
                Vector2 forceDirection = transform.up;
                float targetSpeed = throttleInput * carData.maxSpeed;

                Vector2 targetVelocity = forceDirection * targetSpeed;
                Vector2 velocityDiff = targetVelocity - rb.velocity;

                Vector2 acceleration = velocityDiff * carData.acceleration * Time.fixedDeltaTime;
                rb.velocity += acceleration;
            }

            if (isBraking)
            {
                rb.velocity *= (1f - carData.brakePower * Time.fixedDeltaTime);
            }
        }

        private void ApplySteering()
        {
            if (Mathf.Abs(steeringInput) > 0.01f && currentSpeed > 0.5f)
            {
                float turn = steeringInput * currentTurnRate * Time.fixedDeltaTime;
                float rotationAngle = -turn;
                rb.MoveRotation(rb.rotation + rotationAngle);
            }
        }

        private void ApplyFriction()
        {
            Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.velocity, transform.up);
            Vector2 rightVelocity = transform.right * Vector2.Dot(rb.velocity, transform.right);

            rb.velocity = forwardVelocity + rightVelocity * (1f - currentFriction);

            rb.velocity *= carData.velocityDamping;
        }

        private void UpdateSpeed()
        {
            currentSpeed = rb.velocity.magnitude;
        }

        public float GetDriftAngle()
        {
            if (rb.velocity.magnitude < 0.1f) return 0f;

            return Vector2.SignedAngle(rb.velocity, transform.up);
        }

        public void ResetPhysics()
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            currentSpeed = 0f;
        }
    }
}

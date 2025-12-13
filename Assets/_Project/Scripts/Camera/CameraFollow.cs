using UnityEngine;

namespace DriftRacer.CameraSystem
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform target;

        [Header("Follow Settings")]
        [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);
        [SerializeField] private float smoothSpeed = 5f;
        [SerializeField] private bool useFixedUpdate = false;

        [Header("Look Ahead")]
        [SerializeField] private bool useLookAhead = true;
        [SerializeField] private float lookAheadDistance = 2f;
        [SerializeField] private float lookAheadSpeed = 3f;

        [Header("Boundaries")]
        [SerializeField] private bool useBoundaries = false;
        [SerializeField] private Vector2 minBoundary;
        [SerializeField] private Vector2 maxBoundary;

        [Header("Zoom")]
        [SerializeField] private Camera cam;
        [SerializeField] private float defaultOrthographicSize = 5f;

        private Vector3 currentVelocity = Vector3.zero;
        private Vector3 lookAheadPos = Vector3.zero;

        private void Start()
        {
            if (cam == null)
            {
                cam = GetComponent<Camera>();
            }

            if (cam != null)
            {
                cam.orthographicSize = defaultOrthographicSize;
            }
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }

        private void Update()
        {
            if (!useFixedUpdate)
            {
                FollowTarget();
            }
        }

        private void FixedUpdate()
        {
            if (useFixedUpdate)
            {
                FollowTarget();
            }
        }

        private void FollowTarget()
        {
            if (target == null) return;

            Vector3 targetPosition = target.position + offset;

            if (useLookAhead)
            {
                Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
                if (rb != null && rb.velocity.magnitude > 0.5f)
                {
                    Vector3 lookAheadTarget = (Vector3)rb.velocity.normalized * lookAheadDistance;
                    lookAheadPos = Vector3.Lerp(lookAheadPos, lookAheadTarget, lookAheadSpeed * Time.deltaTime);
                    targetPosition += lookAheadPos;
                }
            }

            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, 1f / smoothSpeed);

            if (useBoundaries)
            {
                smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minBoundary.x, maxBoundary.x);
                smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minBoundary.y, maxBoundary.y);
            }

            transform.position = smoothedPosition;
        }

        public void SetOrthographicSize(float size)
        {
            if (cam != null)
            {
                cam.orthographicSize = size;
            }
        }

        public void ResetCamera()
        {
            if (target != null)
            {
                transform.position = target.position + offset;
                currentVelocity = Vector3.zero;
                lookAheadPos = Vector3.zero;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (useBoundaries)
            {
                Gizmos.color = Color.yellow;
                Vector3 bottomLeft = new Vector3(minBoundary.x, minBoundary.y, 0f);
                Vector3 topRight = new Vector3(maxBoundary.x, maxBoundary.y, 0f);
                Vector3 topLeft = new Vector3(minBoundary.x, maxBoundary.y, 0f);
                Vector3 bottomRight = new Vector3(maxBoundary.x, minBoundary.y, 0f);

                Gizmos.DrawLine(bottomLeft, topLeft);
                Gizmos.DrawLine(topLeft, topRight);
                Gizmos.DrawLine(topRight, bottomRight);
                Gizmos.DrawLine(bottomRight, bottomLeft);
            }
        }
    }
}

using UnityEngine;

namespace DriftRacer.Car
{
    [RequireComponent(typeof(DriftController))]
    public class TireMarks : MonoBehaviour
    {
        [Header("References")]
        private DriftController driftController;
        private TrailRenderer leftTrail;
        private TrailRenderer rightTrail;

        [Header("Settings")]
        [SerializeField] private float trailWidth = 0.2f;
        [SerializeField] private float trailTime = 2f;
        [SerializeField] private Color trailColor = new Color(0.1f, 0.1f, 0.1f, 0.8f);

        private void Awake()
        {
            driftController = GetComponent<DriftController>();
            CreateTrailRenderers();
        }

        private void CreateTrailRenderers()
        {
            GameObject leftTire = new GameObject("LeftTireMark");
            leftTire.transform.SetParent(transform);
            leftTire.transform.localPosition = new Vector3(-0.3f, -0.5f, 0f);
            leftTrail = leftTire.AddComponent<TrailRenderer>();

            GameObject rightTire = new GameObject("RightTireMark");
            rightTire.transform.SetParent(transform);
            rightTire.transform.localPosition = new Vector3(0.3f, -0.5f, 0f);
            rightTrail = rightTire.AddComponent<TrailRenderer>();

            ConfigureTrail(leftTrail);
            ConfigureTrail(rightTrail);
        }

        private void ConfigureTrail(TrailRenderer trail)
        {
            trail.time = trailTime;
            trail.startWidth = trailWidth;
            trail.endWidth = 0f;
            trail.material = new Material(Shader.Find("Sprites/Default"));
            trail.startColor = trailColor;
            trail.endColor = new Color(trailColor.r, trailColor.g, trailColor.b, 0f);
            trail.emitting = false;
            trail.sortingOrder = -1;
        }

        private void Update()
        {
            if (leftTrail != null && rightTrail != null)
            {
                bool shouldEmit = driftController.IsDrifting;
                leftTrail.emitting = shouldEmit;
                rightTrail.emitting = shouldEmit;
            }
        }
    }
}

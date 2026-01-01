using UnityEngine;
using TMPro;

namespace DriftRacer.Car
{
    [RequireComponent(typeof(DriftController))]
    public class DriftDebugDisplay : MonoBehaviour
    {
        [Header("References")]
        private DriftController driftController;
        private CarPhysics carPhysics;

        [Header("Visual Feedback")]
        [SerializeField] private SpriteRenderer carSprite;
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color driftColor = Color.cyan;

        private void Awake()
        {
            driftController = GetComponent<DriftController>();
            carPhysics = GetComponent<CarPhysics>();

            if (carSprite == null)
            {
                carSprite = GetComponent<SpriteRenderer>();
            }
        }

        private void Update()
        {
            if (driftController.IsDrifting)
            {
                if (carSprite != null)
                {
                    carSprite.color = Color.Lerp(carSprite.color, driftColor, Time.deltaTime * 10f);
                }
            }
            else
            {
                if (carSprite != null)
                {
                    carSprite.color = Color.Lerp(carSprite.color, normalColor, Time.deltaTime * 5f);
                }
            }
        }

        private void OnGUI()
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 20;
            style.normal.textColor = Color.white;

            int y = 10;
            GUI.Label(new Rect(10, y, 300, 30), $"Speed: {carPhysics.CurrentSpeed:F1}", style);
            y += 30;

            // Check input
            bool spacePressed = UnityEngine.Input.GetKey(KeyCode.Space);
            style.normal.textColor = spacePressed ? Color.green : Color.red;
            GUI.Label(new Rect(10, y, 400, 30), $"Spacebar: {(spacePressed ? "PRESSED" : "not pressed")}", style);
            y += 30;

            // Check InputManager
            bool hasInputManager = DriftRacer.Input.InputManager.Instance != null;
            style.normal.textColor = hasInputManager ? Color.green : Color.red;
            GUI.Label(new Rect(10, y, 400, 30), $"InputManager: {(hasInputManager ? "OK" : "MISSING!")}", style);
            y += 30;

            if (hasInputManager)
            {
                bool handbrakeFromManager = DriftRacer.Input.InputManager.Instance.IsHandbrakePressed();
                style.normal.textColor = handbrakeFromManager ? Color.green : Color.red;
                GUI.Label(new Rect(10, y, 400, 30), $"Handbrake Input: {(handbrakeFromManager ? "TRUE" : "false")}", style);
                y += 30;
            }

            // Check drift angle
            float driftAngle = carPhysics.GetDriftAngle();
            style.normal.textColor = Color.white;
            GUI.Label(new Rect(10, y, 400, 30), $"Drift Angle: {driftAngle:F1}°", style);
            y += 30;

            if (driftController.IsDrifting)
            {
                style.normal.textColor = Color.cyan;
                GUI.Label(new Rect(10, y, 300, 30), "DRIFTING!", style);
                y += 30;
                GUI.Label(new Rect(10, y, 400, 30), $"Drift Time: {driftController.DriftTime:F2}s", style);
                y += 30;
                GUI.Label(new Rect(10, y, 400, 30), $"Quality: {driftController.DriftQuality:F0}", style);
                y += 30;
            }
            else
            {
                style.normal.textColor = Color.gray;
                GUI.Label(new Rect(10, y, 300, 30), "Not drifting", style);
                y += 30;
            }

            style.fontSize = 16;
            style.normal.textColor = Color.yellow;
            GUI.Label(new Rect(10, y, 500, 30), "Controls: Arrow Keys to steer, SPACE to drift", style);
        }
    }
}

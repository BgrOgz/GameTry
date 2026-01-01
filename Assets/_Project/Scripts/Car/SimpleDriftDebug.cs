using UnityEngine;

namespace DriftRacer.Car
{
    public class SimpleDriftDebug : MonoBehaviour
    {
        private CarController carController;
        private SpriteRenderer sprite;

        private void Awake()
        {
            carController = GetComponent<CarController>();
            sprite = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            // Force color change based on drift
            if (carController != null && carController.Drift != null)
            {
                if (carController.Drift.IsDrifting)
                {
                    sprite.color = Color.cyan;
                }
                else
                {
                    sprite.color = Color.white;
                }
            }

            // Check if spacebar is being pressed
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("SPACEBAR PRESSED!");
            }

            if (Input.GetKey(KeyCode.Space))
            {
                Debug.Log("SPACEBAR HELD!");
            }
        }

        private void OnGUI()
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 24;
            style.normal.textColor = Color.white;

            int y = 10;

            // Speed
            if (carController != null && carController.Physics != null)
            {
                GUI.Label(new Rect(10, y, 400, 30), $"Speed: {carController.Physics.CurrentSpeed:F2}", style);
                y += 35;
            }

            // Spacebar check
            bool spacePressed = Input.GetKey(KeyCode.Space);
            style.normal.textColor = spacePressed ? Color.green : Color.red;
            GUI.Label(new Rect(10, y, 400, 30), $"SPACEBAR: {(spacePressed ? "PRESSED" : "NOT PRESSED")}", style);
            y += 35;

            // Input Manager Check
            bool hasInputManager = DriftRacer.Input.InputManager.Instance != null;
            style.normal.textColor = hasInputManager ? Color.green : Color.red;
            GUI.Label(new Rect(10, y, 400, 30), $"InputManager: {(hasInputManager ? "EXISTS" : "MISSING!!!")}", style);
            y += 35;

            if (hasInputManager)
            {
                bool handbrake = DriftRacer.Input.InputManager.Instance.IsHandbrakePressed();
                style.normal.textColor = handbrake ? Color.green : Color.red;
                GUI.Label(new Rect(10, y, 400, 30), $"Handbrake from Manager: {(handbrake ? "TRUE" : "FALSE")}", style);
                y += 35;
            }

            // Drift angle
            if (carController != null && carController.Physics != null)
            {
                float driftAngle = carController.Physics.GetDriftAngle();
                style.normal.textColor = Color.white;
                GUI.Label(new Rect(10, y, 400, 30), $"Drift Angle: {driftAngle:F1}°", style);
                y += 35;
            }

            // IS DRIFTING?
            if (carController != null && carController.Drift != null)
            {
                bool isDrifting = carController.Drift.IsDrifting;
                style.fontSize = 36;
                style.normal.textColor = isDrifting ? Color.cyan : Color.gray;
                GUI.Label(new Rect(10, y, 400, 50), isDrifting ? ">>> DRIFTING! <<<" : "Not drifting", style);
                y += 55;

                if (isDrifting)
                {
                    style.fontSize = 24;
                    GUI.Label(new Rect(10, y, 400, 30), $"Drift Time: {carController.Drift.DriftTime:F2}s", style);
                    y += 35;
                    GUI.Label(new Rect(10, y, 400, 30), $"Quality: {carController.Drift.DriftQuality:F0}", style);
                }
            }

            // Instructions
            style.fontSize = 18;
            style.normal.textColor = Color.yellow;
            y += 40;
            GUI.Label(new Rect(10, y, 600, 30), "Press SPACE + ARROW KEY to drift!", style);
        }
    }
}

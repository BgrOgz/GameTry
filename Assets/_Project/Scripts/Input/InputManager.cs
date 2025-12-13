using UnityEngine;

namespace DriftRacer.Input
{
    public enum ControlScheme
    {
        Buttons,
        SteeringWheel
    }

    public class InputManager : MonoBehaviour
    {
        private static InputManager instance;
        public static InputManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("InputManager");
                    instance = go.AddComponent<InputManager>();
                }
                return instance;
            }
        }

        [Header("Control Scheme")]
        [SerializeField] private ControlScheme currentScheme = ControlScheme.Buttons;

        [Header("Input Providers")]
        private IInputProvider currentProvider;
        private ButtonInputProvider buttonProvider;
        private SteeringWheelInputProvider wheelProvider;

        public ControlScheme CurrentScheme => currentScheme;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeProviders();
        }

        private void InitializeProviders()
        {
            buttonProvider = gameObject.AddComponent<ButtonInputProvider>();
            wheelProvider = gameObject.AddComponent<SteeringWheelInputProvider>();

            SetControlScheme(currentScheme);
        }

        public void SetControlScheme(ControlScheme scheme)
        {
            currentScheme = scheme;

            switch (scheme)
            {
                case ControlScheme.Buttons:
                    currentProvider = buttonProvider;
                    break;
                case ControlScheme.SteeringWheel:
                    currentProvider = wheelProvider;
                    break;
            }
        }

        public float GetSteering()
        {
            return currentProvider?.GetSteering() ?? 0f;
        }

        public bool IsHandbrakePressed()
        {
            return currentProvider?.IsHandbrakePressed() ?? false;
        }

        public bool IsBrakePressed()
        {
            return currentProvider?.IsBrakePressed() ?? false;
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

namespace DriftRacer.Core
{
    public enum GameState
    {
        MainMenu,
        Garage,
        LevelSelect,
        Playing,
        Paused,
        Results
    }

    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    instance = go.AddComponent<GameManager>();
                }
                return instance;
            }
        }

        [Header("Game State")]
        [SerializeField] private GameState currentState = GameState.MainMenu;

        [Header("Current Game Session")]
        public int currentTrackIndex = 0;
        public int currentCarIndex = 0;
        public int currentScore = 0;
        public float currentTime = 0f;
        public int currentLap = 1;

        public GameState CurrentState => currentState;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            Application.targetFrameRate = 60;
        }

        public void SetGameState(GameState newState)
        {
            currentState = newState;
            OnGameStateChanged(newState);
        }

        private void OnGameStateChanged(GameState newState)
        {
            switch (newState)
            {
                case GameState.Playing:
                    Time.timeScale = 1f;
                    break;
                case GameState.Paused:
                    Time.timeScale = 0f;
                    break;
                case GameState.Results:
                    Time.timeScale = 0f;
                    break;
                default:
                    Time.timeScale = 1f;
                    break;
            }
        }

        public void ResetGameSession()
        {
            currentScore = 0;
            currentTime = 0f;
            currentLap = 1;
        }

        public void QuitGame()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}

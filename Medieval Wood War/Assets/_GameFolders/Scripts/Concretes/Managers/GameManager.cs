using UnityEngine;
using UnityEngine.SceneManagement;

namespace HappyHour.Concretes.Managers
{

    public class GameManager : MonoSingleton<GameManager>
    {
        public enum GameState
        {
            Preparing,
            Playing,
            Finish,
            Failed,
        }

        public float timeLeft = 60;

        [Header("STATE")]
        public GameState gameState;
        // Start is called before the first frame update
        private void Awake()
        {
            Application.targetFrameRate = 150;
        }
        private void Start()
        {
            gameState = GameState.Preparing;
            //PlayerStateManager.Instance.playerState = PlayerStateManager.PlayerState.Idle;
        }
        private void Update()
        {
            Timer();
        }

        public void StartGame()
        {
            gameState = GameState.Playing;
            //PlayerStateManager.Instance.playerState = PlayerStateManager.PlayerState.Moving;
        }
        public void RestartGame()
        {
            SceneManager.LoadScene(0);
        }

        public void FinishGame()
        {
            gameState = GameState.Finish;
            PlayerStateManager.Instance.playerState = PlayerStateManager.PlayerState.Idle;
        }

        void Timer()
        {
            if (GameManager.Instance.gameState == GameManager.GameState.Playing)
            {
                timeLeft -= Time.deltaTime;
                UIManager.Instance.TimeText.text = timeLeft.ToString("0");
                if (timeLeft <= 10)
                {
                    UIManager.Instance.TimeboxRed.SetActive(true);
                    if (timeLeft <= 0)
                    {
                        GameManager.Instance.FinishGame();
                        UIManager.Instance.FinishPanel.SetActive(true);
                    }

                }

            }
        }




    }

}
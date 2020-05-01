using UnityEngine;

namespace KAG.Menu
{
    public class Menu : MonoBehaviour
    {
        private GameManager gameManager;
        private GameSession gameSession;

        private void Start()
        {
            gameManager = GameManager.Instance;
            gameSession = GameManager.Instance.session;
        }

        public void ShowSingleplayer()
        {
            gameManager.StartHost();
        }

        public void ShowMultiplayer()
        {
            gameManager.LoadScene(GameScene.Matchmaking);
        }

        public void Settings()
        {

        }

        public void Logout()
        {
            gameSession.Logout(() =>
            {
                GameManager.Instance.LoadScene(GameScene.Authentication);
            });
        }
    }
}

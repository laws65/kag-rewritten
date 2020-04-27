using UnityEngine;
using UnityEngine.SceneManagement;

namespace KAG.Menu
{
    public class Menu : MonoBehaviour
    {
        private GameSession gameSession;

        private void Start()
        {
            gameSession = GameEngine.Instance.gameSession;
        }

        public void ShowSingleplayer()
        {
            GameEngine.Instance.StartSingleplayer();
        }

        public void ShowMultiplayer()
        {
            GameEngine.Instance.LoadScene(GameScene.Matchmaking);
        }

        public void Settings()
        {

        }

        public void Logout()
        {
            gameSession.Logout(() =>
            {
                GameEngine.Instance.LoadScene(GameScene.Authentication);
            });
        }
    }
}

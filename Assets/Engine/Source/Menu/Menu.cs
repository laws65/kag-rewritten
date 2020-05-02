using UnityEngine;

namespace KAG.Menu
{
    public class Menu : MonoBehaviour
    {
        private GameManager manager;
        private GameSession session;

        private void Start()
        {
            manager = GameManager.Instance;
            session = GameManager.Instance.session;
        }

        public void ShowSingleplayer()
        {
            manager.StartHost();
        }

        public void ShowMultiplayer()
        {
            manager.LoadScene(GameScene.Matchmaking);
        }

        public void Settings()
        {

        }

        public void Logout()
        {
            session.Logout(() =>
            {
                GameManager.Instance.LoadScene(GameScene.Authentication);
            });
        }
    }
}

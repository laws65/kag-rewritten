using UnityEngine;
using UnityEngine.SceneManagement;

namespace KAG.Menu
{
    public class Menu : MonoBehaviour
    {
        public void ShowSingleplayer()
        {
            GameEngine.Instance.StartSingleplayer();
        }

        public void ShowMultiplayer()
        {
            SceneManager.LoadScene(GameEngine.matchmakingScene);
        }

        public void Settings()
        {

        }

        public void Logout()
        {
            GameSession.Instance.Logout(() =>
            {
                SceneManager.LoadScene(GameEngine.authenticationScene);
            });
        }
    }
}

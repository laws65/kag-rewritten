using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace KAG.Menu
{
    public class MatchmakingMenu : MonoBehaviour
    {
        private GameEngine gameEngine;
        private GameSession gameSession;

        public GameObject listContent;
        public GameObject listItem;

        [Space]
        public TextMeshProUGUI statsText;

        private void Awake()
        {
            gameEngine = GameEngine.Instance;
            gameSession = GameEngine.Instance.gameSession;
        }

        private void Start()
        {
            Refresh();
        }

        public void Close()
        {
            gameEngine.LoadScene(GameScene.Authentication);
        }

        public void Refresh()
        {
            gameEngine.ShowMessage("Refreshing server list...");

            gameSession.MatchmakeRefresh((serverList) =>
            {
                foreach (Transform child in listContent.transform)
                {
                    Destroy(child.gameObject);
                }

                serverList.Add(new ServerInfo
                {
                    Name = "Localhost",
                    IP = "127.0.0.1"
                });

                foreach (ServerInfo server in serverList)
                {
                    ServerEntry item = Instantiate(listItem, listContent.transform).GetComponent<ServerEntry>();
                    item.serverInfo = server;
                }
            });
        }
    }
}

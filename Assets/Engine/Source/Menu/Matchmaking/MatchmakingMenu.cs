using UnityEngine;
using TMPro;

namespace KAG.Menu
{
    public class MatchmakingMenu : MonoBehaviour
    {
        private GameManager gameManager;
        private GameSession gameSession;

        public GameObject listContent;
        public GameObject listItem;

        [Space]
        public TextMeshProUGUI statsText;

        private void Start()
        {
            gameManager = GameManager.Instance;
            gameSession = GameManager.Instance.session;

            Refresh();
        }

        public void Close()
        {
            gameManager.LoadScene(GameScene.Authentication);
        }

        public void Refresh()
        {
            gameManager.ShowMessage("Refreshing server list...");

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

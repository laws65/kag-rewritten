using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace KAG
{
    public class MatchmakingMenu : MonoBehaviour
    {
        public TextMeshProUGUI statsText;

        [Space]
        public GameObject listContent;
        public GameObject listItem;
        public Button refreshButton;

        [Space]
        public Button playButton;

        private void Awake()
        {
            Refresh();
        }

        public void Refresh()
        {
            GameSession.Instance.MatchmakeRefresh((serverList) =>
            {
                foreach (Transform child in listContent.transform)
                {
                    Destroy(child.gameObject);
                }

                foreach (ServerInfo server in serverList)
                {
                    ServerItem item = Instantiate(listItem, listContent.transform).GetComponent<ServerItem>();
                    item.serverInfo = server;
                }
            });
        }
    }
}

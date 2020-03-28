using UnityEngine;
using TMPro;

namespace KAG
{
    public class ServerEntry : MonoBehaviour
    {
        public TextMeshProUGUI label;
        public ServerInfo serverInfo;

        private void Start()
        {
            label.text = string.Format("{0} ({1})", serverInfo.Name, serverInfo.IP);
        }

        public void Join()
        {
            GameEngine.Instance.StartClient(serverInfo.IP);
        }
    }
}

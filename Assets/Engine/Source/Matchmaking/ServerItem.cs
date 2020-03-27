using UnityEngine;
using TMPro;

namespace KAG
{
    public class ServerItem : MonoBehaviour
    {
        public TextMeshProUGUI label;
        public ServerInfo serverInfo;

        private void Start()
        {
            label.text = string.Format("{0} ({1})", serverInfo.Name, serverInfo.IP);
        }
    }
}

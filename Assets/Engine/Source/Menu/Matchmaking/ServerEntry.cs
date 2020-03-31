using System;
using UnityEngine;
using TMPro;

namespace KAG.Menu
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
            Toast.Instance.Show("Trying to connect to " + serverInfo.IP + "...");
            GameEngine.Instance.StartClient(serverInfo.IP);
        }
    }
}

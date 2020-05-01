using System;
using UnityEngine;
using TMPro;

namespace KAG.Menu
{
    public class ServerEntry : MonoBehaviour
    {
        private GameManager gameManager;

        public TextMeshProUGUI label;
        public ServerInfo serverInfo;

        private void Awake()
        {
            gameManager = GameManager.Instance;
        }

        private void Start()
        {
            label.text = string.Format("{0} ({1})", serverInfo.Name, serverInfo.IP);
        }

        public void Join()
        {
            gameManager.networkAddress = serverInfo.IP;
            gameManager.StartClient();

            gameManager.ShowMessage("Trying to connect to " + serverInfo.IP + "...");
        }
    }
}

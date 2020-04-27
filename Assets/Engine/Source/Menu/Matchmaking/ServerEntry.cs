using System;
using UnityEngine;
using TMPro;

namespace KAG.Menu
{
    public class ServerEntry : MonoBehaviour
    {
        private GameEngine gameEngine;

        public TextMeshProUGUI label;
        public ServerInfo serverInfo;

        private void Awake()
        {
            gameEngine = GameEngine.Instance;
        }

        private void Start()
        {
            label.text = string.Format("{0} ({1})", serverInfo.Name, serverInfo.IP);
        }

        public void Join()
        {
            gameEngine.ShowMessage("Trying to connect to " + serverInfo.IP + "...");
            gameEngine.StartMultiplayerClient(serverInfo.IP);
        }
    }
}

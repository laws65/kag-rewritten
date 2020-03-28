using System;
using System.Linq;
using UnityEngine.SceneManagement;
using Mirror;

namespace KAG
{
    public class GameEngine : Singleton<GameEngine>
    {
        public NetworkManager mirror;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            var args = Environment.GetCommandLineArgs();
            if (args.Contains("-nographics"))
            {
                StartServer();
            }
            else
            {
                SceneManager.LoadScene("Authentication");
            }
        }

        public void StartServer()
        {
            mirror.StartServer();

            GameSession.Instance.LoginAsGuest((playerInfo) =>
            {
                GameSession.Instance.MatchmakeCreate(new ServerInfo
                {
                    Name = "KAG Server"
                });
            });
        }

        public void StartClient(string host_address)
        {
            mirror.networkAddress = host_address;
            mirror.StartClient();
        }
    }
}

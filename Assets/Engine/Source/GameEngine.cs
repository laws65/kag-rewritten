using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

namespace KAG
{
    using KAG.Misc;

    public class GameEngine : Singleton<GameEngine>
    {
        public static string authenticationScene = "Authentication";
        public static string matchmakingScene = "Matchmaking";
        public static string menuScene = "Menu";

        public NetworkManager mirror;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            var args = Environment.GetCommandLineArgs();
            if (args.Contains("-host"))
            {
                StartMultiplayerServer();
            }
            else
            {
                SceneManager.LoadScene(authenticationScene);
            }
        }

        public void StartMultiplayerServer()
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

        public void StartMultiplayerClient(string networkAddress)
        {
            mirror.networkAddress = networkAddress;
            mirror.StartClient();
        }

        public void StartSingleplayer()
        {
            mirror.StartHost();
        }
    }
}

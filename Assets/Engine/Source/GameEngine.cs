using System;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KAG
{
    using KAG.Menu;
    using KAG.Runtime;

    /// <summary>
    /// All scenes in one place helps when some scene's name is changed so we don't have to update each occurrence.
    /// </summary>
    public static class GameScene
    {
        public const string Authentication = "Authentication";
        public const string Matchmaking = "Matchmaking";
        public const string Match = "Match";
        public const string Menu = "Menu";
    }

    public class GameEngine : MonoBehaviour
    {
        #region Singleton
        private static GameEngine _instance;
        public static GameEngine Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameEngine>();
                }

                return _instance;
            }
        }

        private Toast _toast;
        private Toast toast
        {
            get
            {
                if (_toast == null)
                {
                    _toast = FindObjectOfType<Toast>();
                }

                return _toast;
            }
        }
        #endregion

        public GameRuntime gameRuntime;
        public GameSession gameSession;

        public NetworkManager mirror;

        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;

            DontDestroyOnLoad(gameObject);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            switch (scene.name)
            {
                case GameScene.Match:
                    gameRuntime = new GameRuntime(this);
                    gameRuntime.Init();
                    break;
            }
        }

        private void OnSceneUnloaded(Scene scene)
        {

        }

        private void Start()
        {
            gameSession = new GameSession("https", "vamist.dev", 443, "kag-rewritten");

            var args = Environment.GetCommandLineArgs();
            if (args.Contains("-host"))
            {
                StartMultiplayerServer();
            }
            else
            {
                LoadScene(GameScene.Authentication);
            }
        }

        public void LoadScene(string scene)
        {
            SceneManager.LoadScene(scene);
        }

        public void StartMultiplayerServer()
        {
            mirror.StartServer();

            gameSession.LoginAsGuest((playerInfo) =>
            {
                gameSession.MatchmakeCreate(new ServerInfo
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

        public void ShowMessage(string text)
        {
            toast.Show(text);
        }

        public void ShowError(string text)
        {
            toast.Show("Error: " + text);
        }
    }
}

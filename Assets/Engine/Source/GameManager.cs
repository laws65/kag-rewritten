using System;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KAG
{
    using Jint.Native;
    using KAG.Menu;
    using KAG.Runtime;
    using System.Collections;

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

    public class GameManager : NetworkManager
    {
        [Header("References")]
        public Toast toast;
        public GameObject entityPrefab;

        #region Singleton
        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameManager>();
                }

                return _instance;
            }
        }
        #endregion

        private string currentScene;

        public GameSession session;
        public GameEngine engine;
        public GameMain main;

        public override void Start()
        {
            base.Start();

            session = new GameSession("https", "vamist.dev", 443, "kag-rewritten");

            var args = Environment.GetCommandLineArgs();
            if (args.Contains("-host"))
            {
                StartServer();
            }
            else
            {
                LoadScene(GameScene.Authentication);
            }
        }

        private void StartRuntime()
        {
            engine = new GameEngine();
            engine.Import("Main.js");

            main = new GameMain("Main");
            main.Start("Rules/Sandbox/SandboxRules.js");
        }

        private void Instantiate(string classPath)
        {
            GameEntity entity = Instantiate(entityPrefab).GetComponent<GameEntity>();
            entity.Init(classPath);

            NetworkServer.Spawn(entity.gameObject);
        }

        #region Scene management helpers
        public void LoadScene(string scene)
        {
            StartCoroutine(InternalLoadScene(scene));
        }

        private IEnumerator InternalLoadScene(string scene)
        {
            var asyncOperation = SceneManager.LoadSceneAsync(scene);
            while (!asyncOperation.isDone)
            {
                yield return null;
            }
            currentScene = scene;
        }
        #endregion

        public override void OnStartServer()
        {
            base.OnStartServer();
            
            StartRuntime();

            if (mode == NetworkManagerMode.ServerOnly)
            {
                session.LoginAsGuest((playerInfo) =>
                {
                    session.MatchmakeCreate(new ServerInfo
                    {
                        Name = "KAG Server"
                    });
                });
            }
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

            if (mode == NetworkManagerMode.ClientOnly || mode == NetworkManagerMode.Host)
            {
                StartRuntime();
            }
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            base.OnClientDisconnect(conn);
        }

        public override void OnServerConnect(NetworkConnection conn)
        {
            base.OnServerConnect(conn);

            main.OnPlayerConnected(conn.authenticationData as PlayerInfo);
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            base.OnServerDisconnect(conn);

            main.OnPlayerDisconnected(conn.authenticationData as PlayerInfo);
        }

        /// <summary>
        /// Notify of something in-game
        /// </summary>
        /// <param name="text">The message to display</param>
        public void ShowMessage(string text)
        {
            toast.Show(text);
        }

        /// <summary>
        /// Notify of an error in-game
        /// </summary>
        /// <param name="text">The error to display</param>
        public void ShowError(string text)
        {
            toast.Show("Error: " + text);
        }
    }
}

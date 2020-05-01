using System;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KAG
{
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
        private string currentScene;

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

        public GameEngine engine;
        public GameSession session;
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

        #region Scene management helpers
        public void LoadScene(string scene)
        {
            if (!string.IsNullOrEmpty(currentScene))
            {
                UnloadScene(currentScene);
            }
            currentScene = scene;

            StartCoroutine(InternalLoadScene(scene));
        }

        private IEnumerator InternalLoadScene(string scene)
        {
            var asyncOperation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            while (!asyncOperation.isDone)
            {
                yield return null;
            }
        }

        public void UnloadScene(string scene)
        {
            StartCoroutine(InternalUnloadScene(scene));
        }

        private IEnumerator InternalUnloadScene(string scene)
        {
            var asyncOperation = SceneManager.UnloadSceneAsync(scene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

            while (!asyncOperation.isDone)
            {
                yield return null;
            }
        }
        #endregion

        public override void OnStartServer()
        {
            base.OnStartServer();

            if (mode == NetworkManagerMode.ServerOnly || mode == NetworkManagerMode.Host)
            {
                engine = new GameEngine();
                engine.ExecuteFile("Main.js");

                main = new GameMain(this);
                main.Start("");

                session.LoginAsGuest((playerInfo) =>
                {
                    session.MatchmakeCreate(new ServerInfo
                    {
                        Name = "KAG Server"
                    });
                });

                LoadScene(GameScene.Match);
            }
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

            if (mode == NetworkManagerMode.ClientOnly)
            {
                engine = new GameEngine();
                engine.ExecuteFile("Main.js");

                main = new GameMain(this);
                main.Start("");

                LoadScene(GameScene.Match);
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

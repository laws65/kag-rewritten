#if UNITY_STANDALONE
using System.Net.Sockets;
#endif

using System;
using System.Linq;
using Jint;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEngine : MonoBehaviour
{
    public static GameEngine Instance { get; set; }

    public NetworkManager mirror;
    public Engine jint = new Engine();

    private void Awake()
    {
        if (Instance)
        {
            Debug.LogError("This singleton already exists.", this);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(this);
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

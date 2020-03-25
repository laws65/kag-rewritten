using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nakama;
using Nakama.TinyJson;
using UnityEngine;

public class PlayerInfo
{
    public string Username;
    public string UserId;

    public PlayerInfo(ISession session)
    {
        Username = session.Username;
        UserId = session.UserId;
    }
}

public class ServerInfo
{
    public string server_name;
    public string server_ip;
    public int server_port;
}

public class GameSession : MonoBehaviour
{
    public static GameSession Instance { get; set; }

    public PlayerInfo player;

    #region Session events
    public event Action<PlayerInfo> OnLoginSuccess;
    public event Action OnLoginFailure;

    public event Action<List<ServerInfo>> OnMatchmakeRefresh;
    #endregion

    #region API endpoint
    public string api_scheme = "http";
    public string api_host = "127.0.0.1";
    public int api_port = 7350;
    public string api_key = "defaultKey";
    #endregion

    #region Nakama specifics
    Client nakama;
    ISession session;
    #endregion

    private void Awake()
    {
        if (Instance)
        {
            Debug.LogError("This singleton already exists.", this);
            return;
        }
        Instance = this;

        nakama = new Client(api_scheme, api_host, api_port, api_key);
        DontDestroyOnLoad(this);
    }

    #region Authentication helpers
    public async void Login(string email, string password)
    {
        session = await nakama.AuthenticateEmailAsync(email, password, null, false);
        player = new PlayerInfo(session);

        OnLoginSuccess(player);
    }

    public async void Register(string username, string email, string password)
    {
        session = await nakama.AuthenticateEmailAsync(email, password, username, true);
        player = new PlayerInfo(session);

        OnLoginSuccess(player);
    }

    public async void LoginAsGuest()
    {
        string deviceId = PlayerPrefs.GetString("Nakama.DeviceId");
        if (string.IsNullOrEmpty(deviceId))
        {
            deviceId = SystemInfo.deviceUniqueIdentifier;
            PlayerPrefs.SetString("Nakama.DeviceId", deviceId);
        }

        session = await nakama.AuthenticateDeviceAsync(deviceId);
        player = new PlayerInfo(session);

        OnLoginSuccess(player);
    }
    #endregion

    #region Matchmaking helpers
    public async void MatchmakeRefresh()
    {
        var result = await nakama.RpcAsync(session, "get_servers");
        var list = result.Payload.FromJson<List<ServerInfo>>();

        if (list != null)
        {
            OnMatchmakeRefresh.Invoke(list);
        }
    }
    #endregion
}

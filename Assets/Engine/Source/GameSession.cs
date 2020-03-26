﻿using System;
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
    public string Name;
    public string IP;
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
    IClient nakama;
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
    private void Authenticate(ISession p_session)
    {
        if (p_session == null)
        {
            OnLoginFailure();
        }
        else
        {
            session = p_session;
            player = new PlayerInfo(session);

            OnLoginSuccess(player);
        }
    }

    public async void Login(string email, string password)
    {
        Authenticate(await nakama.AuthenticateEmailAsync(email, password, null, false));
    }

    public async void Register(string username, string email, string password)
    {
        Authenticate(await nakama.AuthenticateEmailAsync(email, password, username, true));
    }

    public async void LoginAsGuest()
    {
        string deviceId = PlayerPrefs.GetString("Nakama.DeviceId");
        if (string.IsNullOrEmpty(deviceId))
        {
            deviceId = SystemInfo.deviceUniqueIdentifier;
            PlayerPrefs.SetString("Nakama.DeviceId", deviceId);
        }

        Authenticate(await nakama.AuthenticateDeviceAsync(deviceId));
    }
    #endregion

    #region Matchmaking helpers
    public async void MatchmakeRefresh()
    {
        var result = await nakama.RpcAsync(session, "get_server_list");
        var list = result.Payload.FromJson<List<ServerInfo>>();

        if (list != null)
        {
            OnMatchmakeRefresh.Invoke(list);
        }
    }

    public async void MatchmakeCreate(ServerInfo serverInfo)
    {
        await nakama.RpcAsync(api_key, "create_server", serverInfo.ToJson());
    }
    #endregion
}

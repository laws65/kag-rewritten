using System;
using System.Collections.Generic;
using UnityEngine;
using Nakama;
using Nakama.TinyJson;

namespace KAG
{
    using KAG.Misc;

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

    public class GameSession : Singleton<GameSession>
    {
        public PlayerInfo player;

        #region Session events
        public delegate void OnLoginSuccess(PlayerInfo pinfo);
        public delegate void OnLoginFailure(Exception e);

        public delegate void OnMatchmakeRefresh(List<ServerInfo> serverList);
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

        public bool IsConnected { get => session != null; }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            nakama = new Client(api_scheme, api_host, api_port, api_key, UnityWebRequestAdapter.Instance);
        }

        #region Authentication helpers
        private void Authenticate(ISession p_session)
        {
            if (p_session == null) return;

            session = p_session;
            player = new PlayerInfo(session);
        }

        public async void Login(string email, string password, OnLoginSuccess onSuccess = null, OnLoginFailure onFailure = null)
        {
            try
            {
                Authenticate(await nakama.AuthenticateEmailAsync(email, password, null, false));
                onSuccess?.Invoke(player);
            }
            catch (Exception e)
            {
                onFailure?.Invoke(e);
            }
        }

        public async void Register(string username, string email, string password, OnLoginSuccess onSuccess = null, OnLoginFailure onFailure = null)
        {
            try
            {
                Authenticate(await nakama.AuthenticateEmailAsync(email, password, username, true));
                onSuccess?.Invoke(player);
            }
            catch (Exception e)
            {
                onFailure?.Invoke(e);
            }
        }

        public async void LoginAsGuest(OnLoginSuccess onSuccess = null, OnLoginFailure onFailure = null)
        {
            string deviceId = PlayerPrefs.GetString("Nakama.DeviceId");
            if (string.IsNullOrEmpty(deviceId))
            {
                deviceId = SystemInfo.deviceUniqueIdentifier;
                PlayerPrefs.SetString("Nakama.DeviceId", deviceId);
            }

            try
            {
                Authenticate(await nakama.AuthenticateDeviceAsync(deviceId));
                onSuccess?.Invoke(player);
            }
            catch (Exception e)
            {
                onFailure?.Invoke(e);
            }
        }
        #endregion

        #region Matchmaking helpers
        public async void MatchmakeRefresh(OnMatchmakeRefresh onResult = null)
        {
            var result = await nakama.RpcAsync(session, "get_server_list");
            var list = result.Payload.FromJson<List<ServerInfo>>();

            if (list == null)
            {
                onResult?.Invoke(new List<ServerInfo>());
            }
            else
            {
                onResult?.Invoke(list);
            }
        }

        public async void MatchmakeCreate(ServerInfo serverInfo)
        {
            await nakama.RpcAsync(session, "create_server", serverInfo.ToJson());
        }
        #endregion
    }
}

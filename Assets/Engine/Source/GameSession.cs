using System;
using System.Collections.Generic;
using UnityEngine;
using Nakama;
using Nakama.TinyJson;

namespace KAG
{
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
        public delegate void OnLoginSuccess(PlayerInfo playerInfo);
        public delegate void OnLoginFailure();

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
        }

        private void Start()
        {
            nakama = new Client(api_scheme, api_host, api_port, api_key);
        }

        #region Authentication helpers
        private void Authenticate(ISession p_session, OnLoginSuccess onSuccess = null, OnLoginFailure onFailure = null)
        {
            if (p_session == null)
            {
                onFailure?.Invoke();
            }
            else
            {
                session = p_session;
                player = new PlayerInfo(session);

                onSuccess?.Invoke(player);
            }
        }

        public async void Login(string email, string password, OnLoginSuccess onSuccess = null, OnLoginFailure onFailure = null)
        {
            Authenticate(await nakama.AuthenticateEmailAsync(email, password, null, false), onSuccess, onFailure);
        }

        public async void Register(string username, string email, string password, OnLoginSuccess onSuccess = null, OnLoginFailure onFailure = null)
        {
            Authenticate(await nakama.AuthenticateEmailAsync(email, password, username, true), onSuccess, onFailure);
        }

        public async void LoginAsGuest(OnLoginSuccess onSuccess = null, OnLoginFailure onFailure = null)
        {
            string deviceId = PlayerPrefs.GetString("Nakama.DeviceId");
            if (string.IsNullOrEmpty(deviceId))
            {
                deviceId = SystemInfo.deviceUniqueIdentifier;
                PlayerPrefs.SetString("Nakama.DeviceId", deviceId);
            }

            Authenticate(await nakama.AuthenticateDeviceAsync(deviceId), onSuccess, onFailure);
        }
        #endregion

        #region Matchmaking helpers
        public async void MatchmakeRefresh(OnMatchmakeRefresh onResult = null)
        {
            var result = await nakama.RpcAsync(session, "get_server_list");
            var list = result.Payload.FromJson<List<ServerInfo>>();

            if (list != null)
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
